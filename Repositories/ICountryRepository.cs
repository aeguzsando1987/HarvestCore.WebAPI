using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HarvestCore.WebApi.Entites;


namespace HarvestCore.WebApi.Repositories
{
    public interface ICountryRepository : IGenericRepository<Country>
    {
        // Agregar metodos especificos para el repositorio de paises
        // ejemploTask<Country?> GetCountryByCodeAsync(string code);
    }
}