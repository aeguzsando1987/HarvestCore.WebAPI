using AutoMapper;
using AutoMapper.QueryableExtensions;
using HarvestCore.WebApi.Data;
using HarvestCore.WebApi.DTOs.Harvester;
using HarvestCore.WebApi.Entites;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;






namespace HarvestCore.WebApi.Repositories
{
    public class HarvesterRepository : IHarvesterRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public HarvesterRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ReadHarvesterDto>> GetAllHarvestersAsync(string? name,
                                DateTime? createdBefore,
                                DateTime? createdAfter,
                                string? locality,
                                int? idCrew,
                                string? crewKey)
        {
            var query = _context.Harvesters.AsQueryable();
            if (!string.IsNullOrWhiteSpace(name))
            {
                var nameTerms = name.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var term in nameTerms)
                {
                    query = query.Where(h => h.Name.ToLower().Contains(term.ToLower()));
                }
            }

            if (createdBefore.HasValue)
            {
                query = query.Where(h => h.CreatedAt <= createdBefore.Value);
            }

            if (createdAfter.HasValue)
            {
                query = query.Where(h => h.CreatedAt >= createdAfter.Value);
            }

            if (!string.IsNullOrWhiteSpace(locality))
            {
                var localityTerms = locality.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var term in localityTerms)
                {
                    query = query.Where(h => h.CrewEntity.CommunityEntity.Name.ToLower().Contains(term.ToLower()));
                }
            }

            if (idCrew.HasValue)
            {
                query = query.Where(h => h.CrewEntity.IdCrew == idCrew.Value);
            }

            if (!string.IsNullOrWhiteSpace(crewKey))
            {
                query = query.Where(h => h.CrewEntity.CrewKey.ToLower() == crewKey.ToLower());
            }

            return await query
                        .ProjectTo<ReadHarvesterDto>(_mapper.ConfigurationProvider)
                        .ToListAsync();
        }

        public async Task<ReadHarvesterDto?> GetHarvesterByIdAsync(int id)
        { 
            return await _context.Harvesters
                .Where(h => h.IdHarvester == id)
                .ProjectTo<ReadHarvesterDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
           
        }

        public async Task<ReadHarvesterDto?> GetHarvesterByKeyAsync(string key)
        {
            return await _context.Harvesters
                .Where(h => h.HarvesterKey == key)
                .ProjectTo<ReadHarvesterDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ReadHarvesterDto>> GetHarvestersByCrewKeyAsync(string CrewKey)
        {
            return await _context.Harvesters
                .Where(h => h.CrewEntity.CrewKey == CrewKey)
                .ProjectTo<ReadHarvesterDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<ReadHarvesterDto> CreateHarvesterAsync(CreateHarvesterDto harvesterDto)
        {
            var harvester = _mapper.Map<Harvester>(harvesterDto);
            _context.Harvesters.Add(harvester);
            await _context.SaveChangesAsync();
            return _mapper.Map<ReadHarvesterDto>(harvester);
        }

        public async Task<bool> UpdateHarvesterAsync(int id, UpdateHarvesterDto harvesterDto)
        {
            var harvester = await _context.Harvesters.FindAsync(id);
            if (harvester == null)
            {
                return false;
            }
            _mapper.Map(harvesterDto, harvester);
            _context.Entry(harvester).State = EntityState.Modified;

            try
            {
                return await _context.SaveChangesAsync() > 0;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }

        public async Task<bool> DeleteHarvesterAsync(int id)
        {
            var harvester = await _context.Harvesters.FindAsync(id);
            if (harvester == null)
            {
                return false;
            }
            _context.Harvesters.Remove(harvester);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Harvester?> GetHarvesterEntityByIdAsync(int id)
        {
            return await _context.Harvesters
                .FirstOrDefaultAsync(h => h.IdHarvester == id);
        }

        public async Task<bool> HarvesterExistsAsync(int id)
        {
            return await _context.Harvesters.AnyAsync(h => h.IdHarvester == id);
        }

       

    }
}
