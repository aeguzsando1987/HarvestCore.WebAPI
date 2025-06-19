using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HarvestCore.WebApi.Repositories;
using System.Runtime.CompilerServices;
using AutoMapper;
using HarvestCore.WebApi.DTOs.Crew;
using Microsoft.AspNetCore.JsonPatch;
using HarvestCore.WebApi.Entites;

namespace HarvestCore.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CrewController : ControllerBase
    {
        private readonly ICrewRepository _crewRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CrewController> _logger;

        public CrewController(
            ICrewRepository crewRepository,
            IMapper mapper,
            ILogger<CrewController> logger)
        {
            _crewRepository = crewRepository;
            _mapper = mapper;
            _logger = logger;
        }

        // GET: api/Crew
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ReadCrewDto>>> GetCrews()
        {
            var crews = await _crewRepository.GetAllCrewsAsync();
            return Ok(crews);
        }

        // GET: api/Crew/{id}
        [HttpGet("{id:int}", Name = "GetCrewById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ReadCrewDto>> GetCrewById(int id)
        {
            var crewDto = await _crewRepository.GetCrewByIdAsync(id);
            if (crewDto == null)
            {
                _logger.LogWarning("GetCrewById({Id}) NOT FOUND", id);
                return NotFound();
            }

            return Ok(crewDto);
        }

        // GET: api/Crew/bykey/{key}
        [HttpGet("bykey/{key}", Name = "GetCrewByKey")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ReadCrewDto>> GetCrewByKey(string key)
        {
            var crewDto = await _crewRepository.GetCrewByKeyAsync(key);
            if (crewDto == null)
            {
                _logger.LogWarning("GetCrewByKey({Key}) NOT FOUND", key);
                return NotFound();
            }

            return Ok(crewDto);
        }

        // POST: api/Crew
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)] 
        public async Task<IActionResult> PostCrew([FromBody] CreateCrewDto createCrewDto)
        {
            if (createCrewDto == null)
            {
                return BadRequest("Crew data cannot be null.");
            }

            // VALIDACIÓN: Verificar si ya existe un Crew con la misma clave única.
            var existingCrew = await _crewRepository.GetCrewByKeyAsync(createCrewDto.CrewKey);
            if (existingCrew != null)
            {
                _logger.LogWarning("Attempted to create a crew with a duplicate CrewKey: {CrewKey}", createCrewDto.CrewKey);
                // Devolver 409 Conflict para indicar que el recurso ya existe.
                return Conflict(new { message = $"A crew with key '{createCrewDto.CrewKey}' already exists.", existingCrewId = existingCrew.IdCrew });
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for crew creation: {@ModelState}", ModelState);
                return BadRequest(ModelState);
            }

            // Aquí se crea el nuevo registro de cuadrilla en la base de datos
            // mediante el repositorio, que mapea el DTO a una entidad y la persiste
            var createdCrewDto = await _crewRepository.CreateCrewAsync(createCrewDto);
            
            _logger.LogInformation("Crew created successfully with ID: {CrewId}", createdCrewDto.IdCrew);
            
            // Devolvemos una respuesta 201 Created con la ubicación del recurso creado
            // y el contenido del nuevo registro de cuadrilla
            return CreatedAtAction("GetCrewById", 
                                    new { id = createdCrewDto.IdCrew }, 
                                    createdCrewDto);
        }

        // PUT: api/Crew/{id}
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutCrew(int id, [FromBody] UpdateCrewDto updateCrewDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedCrewDto = await _crewRepository.UpdateCrewAsync(id, updateCrewDto);
            if (updatedCrewDto == null)
            {
                _logger.LogWarning("UpdateCrew({Id}) NOT FOUND", id);
                return NotFound();
            }

            return NoContent();
        }


        // PATCH: api/Crew/{id}
        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PatchCrew(int id,
                                        [FromBody] JsonPatchDocument<UpdateCrewDto> patchDocument)
        {
            if(patchDocument == null)
            {
                _logger.LogWarning("Patch document is null for crew with id: {Id}", id);
                return BadRequest("Patch document cannot be null");
            }

            var existingCrewEntity = await _crewRepository.GetCrewEntityByIdAsync(id);

            if (existingCrewEntity == null)
            {
                _logger.LogWarning("Patch crew({Id}) - Entity not found", id);
                return NotFound();
            }

            var crewToPatch = _mapper.Map<UpdateCrewDto>(existingCrewEntity);
            patchDocument.ApplyTo(crewToPatch, ModelState);

            if (!TryValidateModel(crewToPatch))
            {
                return BadRequest(ModelState);
            }

            var updatedCrewResult = await _crewRepository.UpdateCrewAsync(id, crewToPatch);
            if (updatedCrewResult == null)
            {
                _logger.LogError("PatchCrew({Id}) - Update failied after applying patch", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the crew");
            }

            return NoContent();
        }

        // DELETE: api/Crew/{id}
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCrew(int id)
        {
            var success = await _crewRepository.DeleteCrewAsync(id);
            if(!success)
            {
                _logger.LogWarning("DeleteCrew({Id}) NOT FOUND", id);
                return NotFound();         
            }

            return NoContent();
        }

    }
}