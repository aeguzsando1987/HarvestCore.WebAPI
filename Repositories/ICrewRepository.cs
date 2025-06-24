using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HarvestCore.WebApi.Entites;
using HarvestCore.WebApi.DTOs.Crew;

namespace HarvestCore.WebApi.Repositories
{
    public interface ICrewRepository
    {
        Task<IEnumerable<ReadCrewDto>> GetAllCrewsAsync();
        Task<ReadCrewDto?> GetCrewByIdAsync(int id);
        Task<ReadCrewDto> CreateCrewAsync(CreateCrewDto crewDto);
        Task<ReadCrewDto?> UpdateCrewAsync(int id, UpdateCrewDto crewDto);
        Task<bool> DeleteCrewAsync(int id);
        Task<bool> CrewExistsAsync(int id);
        Task<ReadCrewDto?> GetCrewByKeyAsync(string key);
        Task<Crew?> GetCrewEntityByIdAsync(int id);
        Task<bool> CrewExistsByKeyAsync(string crewKey);
    }
}