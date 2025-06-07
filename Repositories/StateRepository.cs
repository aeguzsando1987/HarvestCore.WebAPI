using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HarvestCore.WebApi.Entites;
using HarvestCore.WebApi.Data;

namespace HarvestCore.WebApi.Repositories
{
    public class StateRepository : GenericRepository<State>, IStateRepository
    {
        public StateRepository(ApplicationDbContext context) : base(context)
        {

        }

        // TODO: Implementar metodos especificos para el repositorio de estados
        // Ejemplos:
        // public Task<State?> GetStateByIdCountryAsync(int IdCountry)
        // {
        //     return await _dbSet.Where(s => s.IdCountry == IdCountry).ToListAsync();
        // }
        //
        // public Task<State?> GetStateByCodeAsync(string code)
        // {
        //     return await _dbSet.Where(s => s.Code == code).ToListAsync();
        // }
    }
}