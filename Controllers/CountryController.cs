using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HarvestCore.WebApi.DTOs.Country;
using HarvestCore.WebApi.Entites;
using HarvestCore.WebApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HarvestCore.WebApi.Data;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;

namespace HarvestCore.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly ICountryRepository _countryRepository;
        private readonly ApplicationDbContext _context; // Inyectado para PATCH
        private readonly IMapper _mapper;             // Inyectado para PATCH y respuestas
        private readonly ILogger<CountriesController> _logger; // Para logging

        public CountriesController(
            ICountryRepository countryRepository,
            ApplicationDbContext context, 
            IMapper mapper,
            ILogger<CountriesController> logger) 
        {
            _countryRepository = countryRepository;
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        // GET: api/Countries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReadCountryDto>>> GetCountries()
        {
            _logger.LogInformation("Getting all countries");
            var countries = await _countryRepository.GetAllCountriesAsync();
            return Ok(countries);
        }

        // GET: api/Countries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ReadCountryDto>> GetCountry(int id)
        {
            _logger.LogInformation("Getting country with ID: {CountryId}", id);
            var country = await _countryRepository.GetCountryByIdAsync(id);

            if (country == null)
            {
                _logger.LogWarning("Country with ID: {CountryId} not found", id);
                return NotFound("Country not found");
            }
            return Ok(country);
        }


        // POST: api/Countries
        [HttpPost]
        public async Task<ActionResult<ReadCountryDto>> CreateCountry([FromForm] CreateCountryDto createCountryDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // 1. Validación Proactiva: Verificamos si ya existe un país con este código.
                var exists = await _countryRepository.CountryExistsByCodeAsync(createCountryDto.CountryCode);
                if (exists)
                {
                    _logger.LogWarning("Attempted to create a country with a duplicate code: {CountryCode}", createCountryDto.CountryCode);
                    return Conflict($"A country with code '{createCountryDto.CountryCode}' already exists.");
                }

                // 2. Creación del país si no existe.
                var createdCountry = await _countryRepository.CreateCountryAsync(createCountryDto);
                _logger.LogInformation("Country created successfully with ID: {CountryId}", createdCountry.IdCountry);
                return CreatedAtAction(nameof(GetCountry), new { id = createdCountry.IdCountry }, createdCountry);
            }
            catch (Exception ex)
            {
                // 3. Captura de Errores Inesperados: Si algo más falla (ej. DB offline), lo capturamos aquí.
                _logger.LogError(ex, "An unexpected error occurred while creating country with code {CountryCode}", createCountryDto.CountryCode);
                return StatusCode(500, "An unexpected internal server error occurred. Please try again later.");
            }
        }

        // PUT: api/Countries/5 (Update completo)
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCountry(int id, [FromForm] UpdateCountryDto updateCountryDto)
        {
            _logger.LogInformation("Updating country with ID: {CountryId}", id);
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("PutCountry model validation failed for ID {CountryID}: {@ModelState}", id, ModelState);
                return BadRequest(ModelState);
            }

            var updatedCountryDto = await _countryRepository.UpdateCountryAsync(id, updateCountryDto);
            if (updatedCountryDto == null)
            {
                _logger.LogWarning("Country with ID: {CountryID} not found for update", id);
                return NotFound("Country not found");
            }

            _logger.LogInformation("Country with ID: {CountryId} updated successfully", id);
            return Ok(updatedCountryDto);
        }

        // PATCH: api/Countries/5 (Update parcial)
        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PatchCountry(int id, [FromBody] JsonPatchDocument<UpdateCountryDto> patchDocument)
        {
            // Verificamos si el documento de parche es nulo
            if (patchDocument == null)
            {
                _logger.LogWarning("Patch document is null for country with id: {Id}", id);
                return BadRequest("Patch document cannot be null.");
            }

            // Obtenemos la entidad del país por su ID desde el repositorio
            var countryEntity = await _countryRepository.GetCountryEntityByIdAsync(id);

            // Si no se encuentra la entidad, retornamos 404 Not Found
            if (countryEntity == null)
            {
                _logger.LogWarning("Country with ID: {Id} not found for patching", id);
                return NotFound();
            }

            // Mapeamos la entidad a un DTO para aplicar los cambios del parche
            var countryToPatch = _mapper.Map<UpdateCountryDto>(countryEntity);
            
            // Aplicamos el documento de parche al DTO y capturamos errores en ModelState
            patchDocument.ApplyTo(countryToPatch, ModelState);

            // Validamos el modelo después de aplicar el parche
            // TryValidateModel verifica todas las anotaciones de validación en el DTO
            if (!TryValidateModel(countryToPatch))
            {
                _logger.LogWarning("Validation failed after applying patch to country with id: {Id}", id);
                return ValidationProblem(ModelState);
            }

            // Enviamos el DTO modificado al repositorio para actualizar la entidad en la base de datos
            var updatedCountryResultDto = await _countryRepository.UpdateCountryAsync(id, countryToPatch);

            // Si la actualización falló en el repositorio, retornamos un error 500
            if(updatedCountryResultDto == null)
            {
                _logger.LogError("PatchCountry({Id}) - UpdateCountryAsync returned null.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the country.");
            }

            // Retornamos 204 No Content para indicar que la operación fue exitosa sin contenido de respuesta
            _logger.LogInformation("Country with ID: {Id} patched successfully", id);
            return NoContent();
        }

        // DELETE: api/Countries/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            _logger.LogInformation("Attempting to delete country with ID: {CountryId}", id);
            var result = await _countryRepository.DeleteCountryAsync(id);

            if (!result)
            {
                _logger.LogWarning("Country with ID: {CountryId} not found for deletion", id);
                return NotFound("Country not found");
            }

            _logger.LogInformation("Country with ID: {CountryId} deleted successfully", id);
            return NoContent();
            
        }
    }
}