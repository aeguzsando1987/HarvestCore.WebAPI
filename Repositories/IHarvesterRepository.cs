using HarvestCore.WebApi.DTOs.Harvester;
using HarvestCore.WebApi.Entites;

namespace HarvestCore.WebApi.Repositories
{
    public interface IHarvesterRepository
    {
        Task<IEnumerable<ReadHarvesterDto>> GetAllHarvestersAsync();
        Task<ReadHarvesterDto> GetHarvesterByIdAsync(int id);
        Task<ReadHarvesterDto?> GetHarvesterByKeyAsync(string Key);
        Task<IEnumerable<ReadHarvesterDto>> GetHarvesterByCrewKeyAsync(string CrewKey);
        Task<ReadHarvesterDto> CreateHarvesterAsync(CreateHarvesterDto harvesterDto);
        Task<bool> UpdateHarvesterAsync(int id, UpdateHarvesterDto harvesterDto);
        Task<bool> DeleteHarvesterAsync(int id);
        Task<bool> HarvesterExistsAsync(int id);
        Task<Harvester?> GetHarvesterEntityByIdAsync(int id);

    }
}
