using HarvestCore.WebApi.Entites;

namespace HarvestCore.WebApi.Repositories
{
    public interface IHarvesterRepository : IGenericRepository<Harvester>
    {
        // TODO: Implementar metodos especificos para el repositorio de cosechadores
        // Ejemplo: Task<IEnumerable<Harvester>> GetHarvestersByCrewIdAsync(int crewId);
        // Ejemplo: Task<IEnumerable<Harvester>> GetActiveHarvestersAsync();
    }
}
