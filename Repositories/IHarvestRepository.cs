using HarvestCore.WebApi.Entites;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HarvestCore.WebApi.Repositories
{
    public interface IHarvestRepository : IGenericRepository<Harvest>
    {
        // TODO: Es común necesitar cosechas con todos sus entidades relacionadas cargadas.
        Task<Harvest?> GetHarvestWithDetailsAsync(int id);
        Task<IEnumerable<Harvest>> GetAllHarvestsWithDetailsAsync();

        // TODO: Agregar otros metodos específicos para cosechas si es necesario en el futuro
        // Ejemplo: Task<IEnumerable<Harvest>> GetHarvestsByDateRangeAsync(DateTime startDate, DateTime endDate);
        // Ejemplo: Task<IEnumerable<Harvest>> GetHarvestsByHarvesterIdAsync(int harvesterId);
        // Ejemplo: Task<IEnumerable<Harvest>> GetHarvestsByCropIdAsync(int cropId);
    }
}
