using AutoMapper;
using AutoMapper.QueryableExtensions;
using HarvestCore.WebApi.Data;
using HarvestCore.WebApi.DTOs.Community;
using HarvestCore.WebApi.Entites;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HarvestCore.WebApi.Repositories
{
    public class CommunityRepository : ICommunityRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CommunityRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ReadCommunityDto> CreateCommunityAsync(CreateCommunityDto communityDto)
        {
            var communityEntity = _mapper.Map<Community>(communityDto);
            _context.Communities.Add(communityEntity);
            await _context.SaveChangesAsync();
            var createdCommunityWithDetails = await _context.Communities
                                                    .Include(c => c.State)
                                                    .Include(c => c.Crews)
                                                    .FirstOrDefaultAsync(c => c.IdCommunity == communityEntity.IdCommunity);
            return _mapper.Map<ReadCommunityDto>(createdCommunityWithDetails); // Mapear la entidad guardada para incluir el ID
        }

        public async Task<bool> DeleteCommunityAsync(int id)
        {
            var communityEntity = await _context.Communities.FindAsync(id);
            if (communityEntity == null)
            {
                return false;
            }
            _context.Communities.Remove(communityEntity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<ReadCommunityDto>> GetAllCommunitiesAsync()
        {
            return await _context.Communities
                .Include(c => c.State) // Incluir State para mapear StateName
                .Include(c => c.Crews) // Incluir Crews para mapear NumberOfCrews y la lista de Crews
                .ProjectTo<ReadCommunityDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<ReadCommunityDto?> GetCommunityByIdAsync(int id)
        {
            var communityEntity = await _context.Communities
                .Include(c => c.State) // Incluir State para mapear StateName
                .Include(c => c.Crews)
                .FirstOrDefaultAsync(c => c.IdCommunity == id);

            return communityEntity == null ? null : _mapper.Map<ReadCommunityDto>(communityEntity);
        }

        public async Task<ReadCommunityDto?> UpdateCommunityAsync(int id, UpdateCommunityDto communityDto)
        {
            var communityEntity = await _context.Communities.FindAsync(id);
            if (communityEntity == null)
            {
                return null;
            }

            _mapper.Map(communityDto, communityEntity); // Aplicar cambios del DTO a la entidad existente
            _context.Entry(communityEntity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await CommunityExistsAsync(id))
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }
            return _mapper.Map<ReadCommunityDto>(communityEntity);
        }

        public async Task<bool> CommunityExistsAsync(int id)
        {
            return await _context.Communities.AnyAsync(e => e.IdCommunity == id);
        }
    }
}