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
        private readonly ApplicationDbContext _dbContext; // Para la estrategia PATCH

        public CommunityController(
            ICommunityRepository communityRepository,
            IMapper mapper,
            ILogger<CommunityController> logger,
            ApplicationDbContext dbContext)
        {
            _communityRepository = communityRepository ?? throw new ArgumentNullException(nameof(communityRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
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
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchCommunity(int id, [FromBody] JsonPatchDocument<UpdateCommunityDto> patchDocument)
        {
            if (patchDocument == null)
            {
                _logger.LogWarning("Documento PATCH nulo recibido para comunidad con ID: {Id}", id);
                return BadRequest("El documento PATCH no puede ser nulo.");
            }

            var communityEntity = await _dbContext.Communities.FindAsync(id);
            if (communityEntity == null)
            {
                _logger.LogWarning("Comunidad con ID: {Id} no encontrada para PATCH.", id);
                return NotFound();
            }

            // Mapear la entidad a UpdateCommunityDto para aplicar el parche
            var communityToPatch = _mapper.Map<UpdateCommunityDto>(communityEntity);

            _logger.LogInformation("Aplicando documento PATCH a comunidad con ID: {Id}. Operaciones: {OperationsCount}", id, patchDocument.Operations.Count);
            patchDocument.ApplyTo(communityToPatch, ModelState); // Aplicar cambios y capturar errores de validación del parche

            // Validar el DTO después de aplicar el parche
            if (!TryValidateModel(communityToPatch))
            {
                _logger.LogWarning("Validación fallida después de aplicar PATCH a comunidad con ID {Id}. ModelState: {ModelState}", id, ModelState);
                return ValidationProblem(ModelState);
            }
            
            // Si ModelState es inválido DESPUÉS de ApplyTo, incluso si TryValidateModel(communityToPatch) fue true antes, 
            // esto puede indicar problemas con la aplicación del parche en sí (ej. path inválido).
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState inválido después de aplicar PATCH (posiblemente por ApplyTo) a comunidad con ID {Id}. ModelState: {ModelState}", id, ModelState);
                return ValidationProblem(ModelState); 
            }

            // Mapear los cambios de vuelta a la entidad original
            _mapper.Map(communityToPatch, communityEntity);
            _dbContext.Entry(communityEntity).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation("Comunidad con ID: {Id} actualizada exitosamente mediante PATCH.", id);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!await _communityRepository.CommunityExistsAsync(id))
                {
                    _logger.LogWarning("Error de concurrencia: Comunidad con ID: {Id} no encontrada durante PATCH SaveChanges.", id);
                    return NotFound();
                }
                else
                {
                    _logger.LogError(ex, "Error de concurrencia al actualizar comunidad con ID: {Id} mediante PATCH.", id);
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al guardar cambios para comunidad con ID: {Id} mediante PATCH.", id);
                return StatusCode(500, "Un error inesperado ocurrió al procesar su solicitud.");
            }

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
