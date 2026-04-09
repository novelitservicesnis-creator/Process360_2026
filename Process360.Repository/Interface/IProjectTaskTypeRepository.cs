using Process360.Core.Models;

namespace Process360.Repository.Interface;

public interface IProjectTaskTypeRepository : IRepository<ProjectTaskType>
{
    Task<ProjectTaskType?> GetTaskTypeByNameAsync(string name);
}
