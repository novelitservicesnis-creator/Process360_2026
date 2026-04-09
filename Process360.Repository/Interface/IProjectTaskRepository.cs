using Process360.Core.Models;

namespace Process360.Repository.Interface;

public interface IProjectTaskRepository : IRepository<ProjectTask>
{
    Task<IEnumerable<ProjectTask>> GetTasksByProjectAsync(int projectId);
    Task<IEnumerable<ProjectTask>> GetTasksByAssigneeAsync(int resourceId);
    Task<IEnumerable<ProjectTask>> GetTasksByTypeAsync(int taskTypeId);
    Task<IEnumerable<ProjectTask>> GetOverdueTasksAsync();
    Task<IEnumerable<ProjectTask>> GetTasksBySprint(int sprintId);
}
