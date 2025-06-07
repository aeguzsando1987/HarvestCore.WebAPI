using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HarvestCore.WebApi.Entites;
using HarvestCore.WebApi.Data;

namespace HarvestCore.WebApi.Repositories
{
    public class CommunityRepository : GenericRepository<Community>, ICommunityRepository
    {
        public CommunityRepository(ApplicationDbContext context) : base(context)
        {

        }

        // TODO: Implementar metodos especificos para el repositorio de comunidades
        // Ejemplo:
        // public Task<IEnumerable<Community>> GetCommunitiesByStateIdAsync(int IdState)
        // {
        //     return await _dbSet.Where(c => c.IdState == IdState).ToListAsync();
        // }
        
    }
}