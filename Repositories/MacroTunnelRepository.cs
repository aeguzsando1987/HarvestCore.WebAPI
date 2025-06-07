using HarvestCore.WebApi.Data;
using HarvestCore.WebApi.Entites;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HarvestCore.WebApi.Repositories
{
    public class MacroTunnelRepository : GenericRepository<MacroTunnel>, IMacroTunnelRepository
    {
        public MacroTunnelRepository(ApplicationDbContext context) : base(context)
        {
        }

        // TODO: Implementar metodos especificos para el repositorio de macro tunnels
        // Ejemplo:
        // public async Task<IEnumerable<MacroTunnel>> GetMacroTunnelsByHarvestTableIdAsync(int harvestTableId)
        // {
        //     return await _dbSet.Where(mt => mt.IdHarvestTable == harvestTableId).ToListAsync();
        // }

        // Ejemplo de sobrescribir un método genérico para incluir datos relacionados:
        // public override async Task<MacroTunnel?> GetByIdAsync(int id)
        // {
        //     return await _dbSet.Include(mt => mt.HarvestTable) // Cargar la HarvestTable de forma anticipada
        //                        .FirstOrDefaultAsync(mt => mt.IdMacroTunnel == id);
        // }

        // public override async Task<IEnumerable<MacroTunnel>> GetAllAsync()
        // {
        //     return await _dbSet.Include(mt => mt.HarvestTable) // Cargar la HarvestTable para todos los macro tunnels
        //                        .ToListAsync();
        // }
    }
}
