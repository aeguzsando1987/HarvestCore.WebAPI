using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HarvestCore.WebApi.Entites;
using HarvestCore.WebApi.Data;
using Microsoft.EntityFrameworkCore;

namespace HarvestCore.WebApi.Repositories
{
    public class HarvestRepository : GenericRepository<Harvest>, IHarvestRepository
    {
        public HarvestRepository(ApplicationDbContext context) : base(context)
        {

        }

        public async Task<Harvest?> GetHarvestWithDetailsAsync(int id)
        {
            return await _dbSet
                .Include(h => h.HarvesterEntity)
                    .ThenInclude(harv => harv!.CrewEntity) // Assuming HarvesterEntity is not null, then load its Crew
                .Include(h => h.MacroTunnelEntity)
                    .ThenInclude(mt => mt!.HarvestTable) // Assuming MacroTunnelEntity is not null, then load its HarvestTable
                .Include(h => h.CropEntity) // Include the Crop
                .FirstOrDefaultAsync(h => h.IdHarvest == id);
        }

        public async Task<IEnumerable<Harvest>> GetAllHarvestsWithDetailsAsync()
        {
            return await _dbSet
                .Include(h => h.HarvesterEntity)
                    .ThenInclude(harv => harv!.CrewEntity)
                .Include(h => h.MacroTunnelEntity)
                    .ThenInclude(mt => mt!.HarvestTable)
                .Include(h => h.CropEntity)
                .ToListAsync();
        }
    }
}