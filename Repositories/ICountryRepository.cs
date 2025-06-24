using HarvestCore.WebApi.DTOs.Country; // Para los DTOs
using HarvestCore.WebApi.Entites;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HarvestCore.WebApi.Repositories
{
    public interface ICountryRepository
    {
        Task<ReadCountryDto?> GetCountryByIdAsync(int id);
        Task<IEnumerable<ReadCountryDto>> GetAllCountriesAsync();
        Task<ReadCountryDto> CreateCountryAsync(CreateCountryDto createCountryDto);
        Task<ReadCountryDto?> UpdateCountryAsync(int id, UpdateCountryDto updateCountryDto);
        Task<bool> DeleteCountryAsync(int id);
        Task<bool> CountryExistsAsync(int id); // Podría ser útil
        // Podríamos añadir aquí métodos más específicos si fueran necesarios, 
        // como GetCountryByCodeAsync(string code) si tuviéramos ese campo y DTO.
        Task<Country?> GetCountryEntityByIdAsync(int id);
        Task<bool> CountryExistsByCodeAsync(string code);
        
    }
}