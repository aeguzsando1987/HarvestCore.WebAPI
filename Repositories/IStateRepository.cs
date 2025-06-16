using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HarvestCore.WebApi.DTOs.State;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HarvestCore.WebApi.Repositories
{
    public interface IStateRepository
    {
        Task<ReadStateDto> GetStateByIdAsync(int id);
        Task<IEnumerable<ReadStateDto>> GetAllStatesAsync();
        Task<ReadStateDto> CreateStateAsync(CreateStateDto createStateDto);
        Task<ReadStateDto?> UpdateStateAsync(int id, UpdateStateDto updateStateDto);
        Task<bool> DeleteStateAsync(int IdState);
        Task<bool> StateExistsAsync(int id);

        // Metodos adicionales
        Task<IEnumerable<ReadStateDto>> GetStateByCountryIdAsync(int idCountry);
        
    }
}