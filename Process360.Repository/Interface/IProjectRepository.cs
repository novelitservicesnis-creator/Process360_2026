using Process360.Core.Models;

namespace Process360.Repository.Interface;

public interface IProjectRepository : IRepository<Project>
{
    Task<Project?> GetProjectByCodeAsync(string code);
    Task<IEnumerable<Project>> GetProjectsByCustomerAsync(int customerId);
    Task<IEnumerable<Project>> GetActiveProjectsAsync();
}
