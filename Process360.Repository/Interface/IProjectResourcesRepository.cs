using Process360.Core.Models;

namespace Process360.Repository.Interface;

public interface IProjectResourcesRepository : IRepository<ProjectResources>
{
    Task<IEnumerable<ProjectResources>> GetResourcesByProjectAsync(int projectId);
    Task<IEnumerable<ProjectResources>> GetProjectsByResourceAsync(int resourceId);
    Task<IEnumerable<ProjectResources>> GetResourcesByRoleAsync(string role);
}
