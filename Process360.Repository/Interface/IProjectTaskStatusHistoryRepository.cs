using Process360.Core.Models;

namespace Process360.Repository.Interface;

public interface IProjectTaskStatusHistoryRepository : IRepository<ProjectTaskStatusHistory>
{
    Task<IEnumerable<ProjectTaskStatusHistory>> GetStatusHistoryByTaskAsync(int taskId);
}
