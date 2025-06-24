using AutoMapper;
using HarvestCore.WebApi.DTOs.Harvester;
using HarvestCore.WebApi.Repositories;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HarvestCore.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HarvesterController : ControllerBase
    {
        private readonly IHarvesterRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<HarvesterController> _logger;

        public HarvesterController(IHarvesterRepository repository, IMapper mapper, ILogger<HarvesterController> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReadHarvesterDto>>> GetAllHarvesters()
        {
            var harvesters = await _repository.GetAllHarvestersAsync();
            return Ok(harvesters);
        }

        [HttpGet("{id:int}", Name = "GetHarvesterById")]
        public async Task<ActionResult<ReadHarvesterDto>> GetHarvesterById(int id)
        {
            var harvester = await _repository.GetHarvesterByIdAsync(id);
            if (harvester == null)
            {
                _logger.LogWarning("Harvester with ID: {HarvesterId} nor found", id);
                return NotFound();
            }
            return Ok(harvester);
        }

        [HttpGet("by-key/{harvesterKey}")]
        public async Task<ActionResult<ReadHarvesterDto>> GetHarvesterByKey(string harvesterKey)
        {
            var harvester = await _repository.GetHarvesterByKeyAsync(harvesterKey);
            if (harvester == null)
            {
                _logger.LogWarning("Harvester with key: {HarvesterKey} nor found", harvesterKey);
                return NotFound();
            }
            return Ok(harvester);
        }

        [HttpGet("by-crew/{crewId}")]
        public async Task<ActionResult<IEnumerable<ReadHarvesterDto>>> GetHarvesterByCrew(string crewKey)
        {
            var harvesters = await _repository.GetHarvestersByCrewKeyAsync(crewKey);
            return Ok(harvesters);
        }

        [HttpPost]
        public async Task<ActionResult<ReadHarvesterDto>> CreateHarvester([FromBody] CreateHarvesterDto createDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingHarvester = await _repository.GetHarvesterByKeyAsync(createDto.HarvesterKey);
            if (existingHarvester != null)
            {
                _logger.LogWarning("Attempting to create Harvester with duplicate key: {HarvesterKey} failed", createDto.HarvesterKey);
                return Conflict(new { message = $"A harvester with key '{createDto.HarvesterKey}' already exists." });
            }

            var newHarvester = await _repository.CreateHarvesterAsync(createDto);
            _logger.LogInformation("Harvester created successfully with ID: {HarvesterId}", newHarvester.IdHarvester);
            return CreatedAtRoute("GetHarvesterById", new { id = newHarvester.IdHarvester }, newHarvester);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateHarvester(int id, [FromBody] UpdateHarvesterDto updateDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _repository.UpdateHarvesterAsync(id, updateDto);
            if (!result)
            {
                _logger.LogWarning("Update failed for harvester with ID: {HarvesterID}", id);
                return NotFound();
            }

            return NoContent();
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult> PatchHarvester(int id, [FromBody] JsonPatchDocument<UpdateHarvesterDto> patchDocument)
        {
            if(patchDocument == null)
            {
                _logger.LogWarning("Patch document is null for harvester with id: {Id}", id);
                return BadRequest("Patch document cannot be null");
            }

            var harvesterEntity = await _repository.GetHarvesterEntityByIdAsync(id);

            if (harvesterEntity == null)
            {
                _logger.LogWarning("Patch harvester({Id}) - Entity not found", id);
                return NotFound();
            }

            var harvesterToPatch = _mapper.Map<UpdateHarvesterDto>(harvesterEntity);
            patchDocument.ApplyTo(harvesterToPatch, ModelState);

            if (!TryValidateModel(harvesterToPatch))
            {
                return BadRequest(ModelState);
            }

            var result = await _repository.UpdateHarvesterAsync(id, harvesterToPatch);
            if (!result)
            {
                _logger.LogError("PatchHarvester({Id}) - Update failied after applying patch", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the harvester");
            }

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteHarvester(int id)
        {
            var result = await _repository.DeleteHarvesterAsync(id);
            if (!result)
            {
                _logger.LogWarning("DeleteHarvester({Id}) - Entity not found", id);
                return NotFound();
            }

            return NoContent();
        }
    }
}