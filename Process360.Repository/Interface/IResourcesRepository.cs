using Process360.Core.Models;

namespace Process360.Repository.Interface;

public interface IResourcesRepository : IRepository<Resources>
{
    Task<Resources?> GetResourceByEmailAsync(string email);
    Task<IEnumerable<Resources>> GetActiveResourcesAsync();
    Task<IEnumerable<Resources>> GetResourcesByRoleAsync(string role);
}
