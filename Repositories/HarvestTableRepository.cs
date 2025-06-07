using HarvestCore.WebApi.Data;
using HarvestCore.WebApi.Entites;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HarvestCore.WebApi.Repositories
{
    public class HarvestTableRepository : GenericRepository<HarvestTable>, IHarvestTableRepository
    {
        public HarvestTableRepository(ApplicationDbContext context) : base(context)
        {
        }

        // TODO: Implementar metodos especificos para el repositorio de tablas de cosecha
        // Ejemplo:
        // public async Task<IEnumerable<HarvestTable>> GetActiveHarvestTablesAsync()
        // {
        //     return await _dbSet.Where(ht => ht.IsActive).ToListAsync();
        // }

        // Ejemplo de sobrescribir un método genérico para incluir datos relacionados (e.g., MacroTunnels):
        // public override async Task<HarvestTable?> GetByIdAsync(int id)
        // {
        //     return await _dbSet.Include(ht => ht.MacroTunnels)
        //                        .FirstOrDefaultAsync(ht => ht.IdHarvestTable == id);
        // }

        // public override async Task<IEnumerable<HarvestTable>> GetAllAsync()
        // {
        //     return await _dbSet.Include(ht => ht.MacroTunnels)
        //                        .ToListAsync();
        // }
    }
}
