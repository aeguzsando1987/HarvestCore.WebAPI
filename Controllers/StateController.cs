using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HarvestCore.WebApi.DTOs.State;
using HarvestCore.WebApi.Entites;
using HarvestCore.WebApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HarvestCore.WebApi.Data;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HarvestCore.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StateController :ControllerBase
    {
        private readonly IStateRepository _stateRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<StateController> _logger;
        private readonly ApplicationDbContext _context; // DbContext para operaciones de PATCH

        public StateController(IStateRepository stateRepository,
                            IMapper mapper,
                            ILogger<StateController> logger,
                            ApplicationDbContext context)
        {
            _stateRepository = stateRepository;
            _mapper = mapper;
            _logger = logger;
            _context = context; // Para PATCH
        }

        // GET: api/States
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReadStateDto>>> GetStates()
        {
            _logger.LogInformation("GEtting states data");
            var states = await _stateRepository.GetAllStatesAsync();
            return Ok(states);
        }

        // Get: api/States/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ReadStateDto>> GetState(int id)
        {
            _logger.LogInformation("Getting the state with id: {Id}", id);
            var state = await _stateRepository.GetStateByIdAsync(id);
            if (state == null)
            {
                _logger.LogWarning("State with id: {Id} not found", id);
                return NotFound("State not found");
            }
            return Ok(state);
        }

        // GET: api/States/ByCountry/{countryId}
        [HttpGet("ByCountry/{countryId}")]
        public async Task<ActionResult<IEnumerable<ReadStateDto>>> GetStatesByCountry(int countryId)
        {
            _logger.LogInformation("Getting states by country id: {CountryId}", countryId);
            var states = await _stateRepository.GetStateByCountryIdAsync(countryId);
            if (!states.Any())
            {
                // TODO: Considerar encviar una lista vacia, ademas de el mesnaje de error
                _logger.LogInformation("States for country with id: {CountryId} not found", countryId);
            }
            return Ok(states);
        }

        // POST: api/States
        [HttpPost]
        public async Task<ActionResult<ReadStateDto>> CreateState(CreateStateDto createStateDto)
        {
            _logger.LogInformation("Creating new state");
            var newState = await _stateRepository.CreateStateAsync(createStateDto);
            return CreatedAtAction(nameof(GetState), new { id = newState.IdState }, newState);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutState(int id, UpdateStateDto updateStateDto)
        {
            _logger.LogInformation("Updating state with ID: {Id}", id);
            // TODO: Validar si el Id en el DTO (si existiera) coincide con el Id de la ruta.
            // if (id != updateStateDto.IdState) // Asumiendo que UpdateStateDto tuviera IdState
            // {
            //     _logger.LogWarning("Conflicto de ID en la solicitud PUT para el estado ID: {Id}.", id);
            //     return BadRequest("El ID en la URL no coincide con el ID en el cuerpo de la solicitud.");
            // }

            var updatedState = await _stateRepository.UpdateStateAsync(id, updateStateDto);
            if(updatedState == null)
            {
                _logger.LogWarning("State with id: {Id} not found during the update", id);
                return NotFound("Not found");
            }
            return Ok(updatedState);
        }

        // PATCH: api/States/{id}
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchState(int id, [FromBody] JsonPatchDocument<UpdateStateDto> patchDocument)
        {
            _logger.LogInformation("Updating parcially state with ID: {Id}", id);
            if (patchDocument == null)
            {
                _logger.LogWarning("Patch document is null for state with id: {Id}", id);
                return BadRequest("Patch document cannot be null");
            }

            var stateEntity = await _context.States.FindAsync(id); // Obtener el estado por ID
            if (stateEntity == null)
            {
                _logger.LogWarning("State with ID : {Id} not found for patching", id);
                return NotFound("State not found");
            }

            // Mapear la entidad a UpdateStateDto para aplicar patch
            var stateToPatch = _mapper.Map<UpdateStateDto>(stateEntity);

            patchDocument.ApplyTo(stateToPatch, ModelState); // aplicar patch al DTO

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Patch state model is not valid after applying update to state with id: {Id}", id);
                return BadRequest(ModelState);
            }

            // Volver a mapear los cambios del DTO parchado de vuelta a la entidad original
            _mapper.Map(stateToPatch, stateEntity);

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation("State with ID: {Id} patched successfully", id);
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency exception while patching state ID: {Id}", id);
                return Conflict("The record you attempted to edit was modified by another user after you got the original values. Please review and try again.");
            }

            return Ok(_mapper.Map<ReadStateDto>(stateEntity));
        }

        // DELETE: api/State/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteState(int id)
        {
            _logger.LogInformation("Atempting to delte state with id: {Id}", id);
            var result = await _stateRepository.DeleteStateAsync(id);

            if(!result)
            {
                _logger.LogWarning("State with ID: {Id} not found for deletion", id);
            }

            _logger.LogInformation("State with ID: {Id} deleted successfully", id);
            // Ok(result) devuelve éxito (200) con el resultado de la operación
            return NoContent();
        }
    }
}     