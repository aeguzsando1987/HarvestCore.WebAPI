using HarvestCore.WebApi.Data;
using HarvestCore.WebApi.Entites;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HarvestCore.WebApi.Enums; // Para CropCategory si se usa en metodos específicos

namespace HarvestCore.WebApi.Repositories
{
    public class CropRepository : GenericRepository<Crop>, ICropRepository
    {
        public CropRepository(ApplicationDbContext context) : base(context)
        {
        }

        // TODO: Implementar metodos especificos para el repositorio de cultivos
        // Ejemplo:
        // public async Task<Crop?> GetCropByKeyAsync(string cropKey)
        // {
        //     return await _dbSet.FirstOrDefaultAsync(c => c.CropKey == cropKey);
        // }

        // public async Task<IEnumerable<Crop>> GetCropsByCategoryAsync(CropCategory category)
        // {
        //     return await _dbSet.Where(c => c.Category == category).ToListAsync();
        // }
    }
}
