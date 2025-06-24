using HarvestCore.WebApi.Data;
using HarvestCore.WebApi.Entites;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HarvestCore.WebApi.Enums; // Para CropCategory si se usa en metodos específicos
using HarvestCore.WebApi.DTOs.Crop;
using AutoMapper;

namespace HarvestCore.WebApi.Repositories
{
    public class CropRepository : ICropRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public CropRepository(ApplicationDbContext context, IMapper mapper) 
        {
            _context = context;
            _mapper = mapper;
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
