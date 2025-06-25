using HarvestCore.WebApi.Data;
using HarvestCore.WebApi.Entites;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HarvestCore.WebApi.Enums; // Para CropCategory si se usa en metodos específicos
using HarvestCore.WebApi.DTOs.Crop;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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

        public async Task<IEnumerable<ReadCropDto>> GetAllCropsAsync(string? productName,
                                            string? cropKey,
                                            string? category,
                                            string? season)
        {
            var query = _context.Crops.AsQueryable();
            if(!string.IsNullOrWhiteSpace(productName))
            {
                var nameTerms = productName.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                foreach(var term in nameTerms)
                {
                    query = query.Where(c => c.ProductName.ToLower().Contains(term.ToLower()));
                }
            }

            if (Enum.TryParse<CropCategory>(category, true, out var cropCategory))
            {
                query = query.Where(c => c.Category == cropCategory);
            }

            if (Enum.TryParse<CropSeasons>(season, true, out var cropSeason))
            {
                query = query.Where(c => c.Season == cropSeason);
            }

            if (!string.IsNullOrWhiteSpace(cropKey))
            {
                var keyTerms = cropKey.Trim().Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
                foreach(var term in keyTerms)
                {
                    query = query.Where(c => c.CropKey.ToLower().Contains(term.ToLower()));
                }
            }

            return await query
                .ProjectTo<ReadCropDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<ReadCropDto?> GetCropByIdAsync(int id)
        {
            return await _context.Crops
                .Where(c => c.IdCrop == id)
                .ProjectTo<ReadCropDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }

        public async Task<ReadCropDto?> GetCropByKeyAsync(string key)
        {
            return await _context.Crops
                .Where(c => c.CropKey.ToLower() == key.ToLower())
                .ProjectTo<ReadCropDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }

        public async Task<Crop?> GetCropEntityByIdAsync(int id)
        {
            return await _context.Crops.FindAsync(id);
        }

        public async Task<ReadCropDto> CreateCropAsync(CreateCropDto createDto)
        {
            var crop = _mapper.Map<Crop>(createDto);
            _context.Crops.Add(crop);
            await _context.SaveChangesAsync();
            return _mapper.Map<ReadCropDto>(crop);
        }

        public async Task<bool> UpdateCropAsync(int id, UpdateCropDto updateDto)
        {
            var crop = await _context.Crops.FindAsync(id);
            if (crop == null)
            {
                return false;
            }
            _mapper.Map(updateDto, crop);
            return await _context.SaveChangesAsync() > 0;
        }

         public async Task<bool> DeleteCropAsync(int id)
        {
            var crop = _context.Crops.FindAsync(id);
            if (crop == null)
            {
                return false;
            }
            _context.Remove(crop);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> CropExistsByIdAsync(int id)
        {
            return await _context.Crops.AnyAsync(c => c.IdCrop == id);
        }

        public async Task<bool> CropExistsByKeyAsync(string cropKey)
        {
            return await _context.Crops.AnyAsync(c => c.CropKey.ToLower() == cropKey.ToLower());
        }

    }
}
