using AutoMapper;
using HarvestCore.WebApi.Data;
using HarvestCore.WebApi.DTOs.Community;
using HarvestCore.WebApi.Repositories;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HarvestCore.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommunityController : ControllerBase
    {
        private readonly ICommunityRepository _communityRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CommunityController> _logger;

        public CommunityController(
            ICommunityRepository communityRepository,
            IMapper mapper,
            ILogger<CommunityController> logger)
        {
            _communityRepository = communityRepository ?? throw new ArgumentNullException(nameof(communityRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // GET: api/community
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReadCommunityDto>>> GetCommunities()
        {
            _logger.LogInformation("Obteniendo todas las comunidades");
            var communities = await _communityRepository.GetAllCommunitiesAsync();
            return Ok(communities);
        }

        // GET: api/community/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ReadCommunityDto>> GetCommunity(int id)
        {
            _logger.LogInformation("Obteniendo comunidad con ID: {Id}", id);
            var community = await _communityRepository.GetCommunityByIdAsync(id);
            if (community == null)
            {
                _logger.LogWarning("Comunidad con ID: {Id} no encontrada.", id);
                return NotFound();
            }
            return Ok(community);
        }

        // POST: api/community
        [HttpPost]
        public async Task<ActionResult<ReadCommunityDto>> PostCommunity([FromBody] CreateCommunityDto createCommunityDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState inválido al crear comunidad: {ModelState}", ModelState);
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Creando nueva comunidad: {CommunityName}", createCommunityDto.Name);
            var createdCommunity = await _communityRepository.CreateCommunityAsync(createCommunityDto);
            _logger.LogInformation("Comunidad creada con ID: {Id}", createdCommunity.IdCommunity);

            return CreatedAtAction(nameof(GetCommunity), new { id = createdCommunity.IdCommunity }, createdCommunity);
        }

        // PUT: api/community/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCommunity(int id, [FromBody] UpdateCommunityDto updateCommunityDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState inválido al actualizar comunidad con ID {Id}: {ModelState}", id, ModelState);
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Actualizando comunidad con ID: {Id}", id);
            var updatedCommunity = await _communityRepository.UpdateCommunityAsync(id, updateCommunityDto);

            if (updatedCommunity == null)
            {
                _logger.LogWarning("Comunidad con ID: {Id} no encontrada durante la actualización.", id);
                return NotFound();
            }
            _logger.LogInformation("Comunidad con ID: {Id} actualizada exitosamente.", id);
            return NoContent(); // O Ok(updatedCommunity) si prefieres devolver la entidad actualizada
        }

        // PATCH: api/community/{id}
        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PatchCommunity(int id, [FromBody] JsonPatchDocument<UpdateCommunityDto> patchDocument)
        {
            if (patchDocument == null)
            {
                _logger.LogWarning("Documento PATCH nulo recibido para comunidad con ID: {Id}", id);
                return BadRequest("El documento PATCH no puede ser nulo.");
            }

            var communityEntity = await _communityRepository.GetCommunityEntityByIdAsync(id);
            if (communityEntity == null)
            {
                _logger.LogWarning("Comunidad con ID: {Id} no encontrada para PATCH.", id);
                return NotFound();
            }

            // Mapear la entidad a UpdateCommunityDto para aplicar el parche
            var communityToPatch = _mapper.Map<UpdateCommunityDto>(communityEntity);

            _logger.LogInformation("Aplicando documento PATCH a comunidad con ID: {Id}. Operaciones: {OperationsCount}", id, patchDocument.Operations.Count);
            patchDocument.ApplyTo(communityToPatch, ModelState); // Aplicar cambios y capturar errores de validación del parche

            // Validar el DTO después de aplicar el parche. 
            // ModelState contendrá errores de ApplyTo y de la validación de atributos del DTO.
            if (!TryValidateModel(communityToPatch))
            {
                _logger.LogWarning("Validación fallida después de aplicar PATCH a comunidad con ID {Id}. ModelState: {ModelState}", id, ModelState);
                return ValidationProblem(ModelState); // Or BadRequest(ModelState)
            }
            
            // El repositorio se encarga de mapear el DTO a la entidad y guardar los cambios.
            var updatedCommunityResultDto = await _communityRepository.UpdateCommunityAsync(id, communityToPatch);

            if (updatedCommunityResultDto == null)
            {
                // Esto podría indicar que la entidad no se encontró durante la actualización en el repositorio,
                // o algún otro error al guardar.
                _logger.LogError("PatchCommunity({Id}) - UpdateCommunityAsync devolvió null después de aplicar el parche.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al actualizar la comunidad.");
            }

            _logger.LogInformation("Comunidad con ID: {Id} actualizada exitosamente mediante PATCH.", id);
            return NoContent();
        }

        // DELETE: api/community/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCommunity(int id)
        {
            _logger.LogInformation("Intentando eliminar comunidad con ID: {Id}", id);
            var success = await _communityRepository.DeleteCommunityAsync(id);
            if (!success)
            {
                _logger.LogWarning("Comunidad con ID: {Id} no encontrada para eliminar.", id);
                return NotFound();
            }
            _logger.LogInformation("Comunidad con ID: {Id} eliminada exitosamente.", id);
            return NoContent();
        }
    }
}
