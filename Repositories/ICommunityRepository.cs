using HarvestCore.WebApi.DTOs.Community;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HarvestCore.WebApi.Repositories
{
    public interface ICommunityRepository
    {
        Task<IEnumerable<ReadCommunityDto>> GetAllCommunitiesAsync();
        Task<ReadCommunityDto?> GetCommunityByIdAsync(int id);
        Task<ReadCommunityDto> CreateCommunityAsync(CreateCommunityDto communityDto);
        Task<ReadCommunityDto?> UpdateCommunityAsync(int id, UpdateCommunityDto communityDto);
        Task<bool> DeleteCommunityAsync(int id);
        Task<bool> CommunityExistsAsync(int id); // Útil para validaciones en el controlador
    }
}
