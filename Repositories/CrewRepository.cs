using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HarvestCore.WebApi.Entites;
using HarvestCore.WebApi.Data;

namespace HarvestCore.WebApi.Repositories
{
    public class CrewRepository : GenericRepository<Crew>, ICrewRepository
    {
        public CrewRepository(ApplicationDbContext context) : base(context)
        {

        }

        // TODO: Implementar metodos especificos para el repositorio de crews
        // Ejemplo:
        // public Task<IEnumerable<Crew>> GetCrewsByCommunityIdAsync(int IdCommunity)
        // {
        //     return await _dbSet.Where(c => c.IdCommunity == IdCommunity).ToListAsync();
        // }
    }
}