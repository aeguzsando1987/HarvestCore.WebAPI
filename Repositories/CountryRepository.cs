using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HarvestCore.WebApi.Entites;
using HarvestCore.WebApi.Data;

namespace HarvestCore.WebApi.Repositories
{
    public class CountryRepository : GenericRepository<Country>, ICountryRepository
    {
        public CountryRepository(ApplicationDbContext context) : base(context)
        {

        }

        // TODO: Implementar metodos especificos para el repositorio de paises
        // Ejemplo:
        // public Task<Country?> GetCountryByCodeAsync(string code)
        // {
        //     return await _dbSet.FirstOrDefaultAsync(c => c.Code == code);
        // }
    }
}