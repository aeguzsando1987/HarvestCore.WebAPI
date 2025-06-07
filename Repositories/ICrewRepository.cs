using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HarvestCore.WebApi.Entites;

namespace HarvestCore.WebApi.Repositories
{
    public interface ICrewRepository : IGenericRepository<Crew>
    {
        // TODO: Implementar metodos especificos para el repositorio de crews
        // Ejemplo:
        // public Task<IEnumerable<Crew>> GetCrewsByCommunityIdAsync(int IdCommunity);
    }
}