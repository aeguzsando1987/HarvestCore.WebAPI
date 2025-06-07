using HarvestCore.WebApi.Entites;

namespace HarvestCore.WebApi.Repositories
{
    public interface ICommunityRepository : IGenericRepository<Community>
    {
        // Add any community-specific methods here if needed in the future
        // Example: Task<IEnumerable<Community>> GetCommunitiesByStateIdAsync(int IdState);
    }
}
