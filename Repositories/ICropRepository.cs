using HarvestCore.WebApi.DTOs.Crop;
using HarvestCore.WebApi.Entites;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Options;

namespace HarvestCore.WebApi.Repositories
{
    public interface ICropRepository
    {
        Task<IEnumerable<ReadCropDto>> GetAllCropsAsync(string? productName,
                                            string? cropKey,
                                            string? category,
                                            string? season);
        Task<ReadCropDto?> GetCropByIdAsync(int id);
        Task<ReadCropDto?> GetCropByKeyAsync(string cropKey);
        Task<ReadCropDto> CreateCropAsync(CreateCropDto createDto);
        Task<bool> UpdateCropAsync (int id, UpdateCropDto updateDto);
        Task<bool> DeleteCropAsync(int id);
        Task<Crop?> GetCropEntityByIdAsync(int id);
        Task<bool> CropExistsByIdAsync(int id);
        Task<bool> CropExistsByKeyAsync(string cropKey);
    }
}
