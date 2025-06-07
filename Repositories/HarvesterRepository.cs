using HarvestCore.WebApi.Data;
using HarvestCore.WebApi.Entites;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HarvestCore.WebApi.Repositories
{
    public class HarvesterRepository : GenericRepository<Harvester>, IHarvesterRepository
    {
        public HarvesterRepository(ApplicationDbContext context) : base(context)
        {
        }

        // TODO: Implementar métodos específicos para cosechadores aquí si es necesario en el futuro
        // Ejemplo:
        // public async Task<IEnumerable<Harvester>> GetHarvestersByCrewIdAsync(int crewId)
        // {
        //     return await _dbSet.Where(h => h.IdCrew == crewId).ToListAsync();
        // }

        // public async Task<IEnumerable<Harvester>> GetActiveHarvestersAsync()
        // {
        //     return await _dbSet.Where(h => h.IsActive).ToListAsync();
        // }

        // Ejemplo de sobrescribir un método genérico para incluir datos relacionados:
        // public override async Task<Harvester?> GetByIdAsync(int id)
        // {
        //     return await _dbSet.Include(h => h.Crew) // Cargar la Cuadrilla de forma anticipada
        //                        .FirstOrDefaultAsync(h => h.IdHarvester == id);
        // }

        // public override async Task<IEnumerable<Harvester>> GetAllAsync()
        // {
        //     return await _dbSet.Include(h => h.Crew) // Cargar la Cuadrilla para todos los cosechadores
        //                        .ToListAsync();
        // }
    }
}
