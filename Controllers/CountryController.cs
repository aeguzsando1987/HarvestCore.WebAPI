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
        public async Task<ActionResult<ReadCountryDto>> CreateCountry(CreateCountryDto createCountryDto)
        {
            _logger.LogInformation("Creating country with name: {CountryName}", createCountryDto.CountryCode);
            if(!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for country creation: {@ModelState}", ModelState);
                return BadRequest(ModelState);
            }

            var createdCountry = await _countryRepository.CreateCountryAsync(createCountryDto);
            _logger.LogInformation("Country created successfully with ID: {CountryId}", createdCountry.IdCountry);
            return CreatedAtAction(nameof(GetCountry), new { id = createdCountry.IdCountry }, createdCountry);
        }

        // PUT: api/Countries/5 (Update completo)
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCountry(int id, UpdateCountryDto updateCountryDto)
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
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchCountry(int id, JsonPatchDocument<UpdateCountryDto> patchDocument)
        {
            _logger.LogInformation("Patching country with ID: {CountryId}", id);
            if (patchDocument == null)
            {
                _logger.LogWarning("Patch document is null for country ID: {CountrId}", id);
                return BadRequest("Patch document cannot be null");
            }

            var countryEntity = await _context.Countries.FindAsync(id);
            if (countryEntity == null)
            {
                _logger.LogWarning("Country with ID: {CountryId} not foud for patching", id);
                return NotFound($"Country with {id} not found");
            }

            // Mapear la entidad a UpdateCountryDto para aplicar patch
            var countryToPatch = _mapper.Map<UpdateCountryDto>(countryEntity);

            // Limpiar cualquier error de ModelState relacionado con 'id' de la ruta,
            // ya que no es parte de UpdateCountryDto y no debe afectar su validación.
            if (ModelState.ContainsKey("id"))
            {
                ModelState.Remove("id");
                _logger.LogInformation("Removed 'id' from ModelState before applying patch.");
            }
            // También podemos quitar el de patchDocument si aparece, aunque es menos común aquí
            if (ModelState.ContainsKey("patchDocument"))
            {
                 ModelState.Remove("patchDocument");
                _logger.LogInformation("Removed 'patchDocument' from ModelState before applying patch.");
            }

            _logger.LogInformation("ModelState BEFORE ApplyTo (after potential 'id' removal): {@ModelState}", ModelState);

            // Aplicar el patch al DTO. Errores se añaden al ModelState
            patchDocument.ApplyTo(countryToPatch, ModelState);
            _logger.LogInformation("ModelState AFTER ApplyTo: {@ModelState}", ModelState);
            _logger.LogInformation("countryToPatch values after ApplyTo - Name: {Name}, CountryCode: {CountryCode}", countryToPatch.Name, countryToPatch.CountryCode);

            // En lugar de TryValidateModel(countryToPatch), solo verificamos si ApplyTo generó errores.
            // Si el parche es válido y se aplica a un DTO que ya era válido (mapeado desde una entidad válida),
            // el resultado debería ser válido respecto a las propiedades afectadas por el parche.
            // Las propiedades no tocadas por el parche conservan su validez original.
            if (!ModelState.IsValid) // Verificar si ApplyTo añadió errores
            {
                _logger.LogWarning("PatchCountry ApplyTo operation resulted in an invalid ModelState for ID {CountryId}: {@ModelState}", id, ModelState);
                return ValidationProblem(ModelState); // Devuelve los errores reportados por ApplyTo
            }

            // Si llegamos aquí, ApplyTo no reportó errores, y asumimos que el countryToPatch es suficientemente válido
            // para proceder con el mapeo a la entidad y guardar.
            _logger.LogInformation("Patch operations applied successfully and ModelState is valid after ApplyTo for ID {CountryId}.", id);

            // Mapear los cambios del DTO parchado de vuelta a la entidad original
            _mapper.Map(countryToPatch, countryEntity);

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation("Country with ID: {CountryId} patched successfully", id);
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency exception while patching country ID: {CountryId}", id);
                return Conflict("The record you attempted to edit was modified by another user after you got the original values. Please review and try again.");
            }

            return Ok(_mapper.Map<ReadCountryDto>(countryEntity));
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