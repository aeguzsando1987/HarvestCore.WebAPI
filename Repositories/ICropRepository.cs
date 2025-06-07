using HarvestCore.WebApi.Entites;

namespace HarvestCore.WebApi.Repositories
{
    public interface ICropRepository : IGenericRepository<Crop>
    {
        // TODO: Implementar metodos especificos para el repositorio de cultivos
        // Ejemplo: Task<Crop?> GetCropByKeyAsync(string cropKey);
        // Ejemplo: Task<IEnumerable<Crop>> GetCropsByCategoryAsync(CropCategory category);
    }
}
