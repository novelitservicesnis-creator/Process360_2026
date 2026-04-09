using Process360.Core.Models;

namespace Process360.Repository.Interface;

public interface IProjectPlanningTasksRepository : IRepository<ProjectPlanningTasks>
{
    Task<IEnumerable<ProjectPlanningTasks>> GetTasksByPlanningAsync(int planningId);
    Task<IEnumerable<ProjectPlanningTasks>> GetTasksByProjectAsync(int projectId);
    Task<IEnumerable<ProjectPlanningTasks>> GetCompletedTasksAsync();
}
