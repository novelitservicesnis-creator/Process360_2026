using Process360.Core.Models;

namespace Process360.Repository.Interface;

public interface IProjectTaskLinkedRepository : IRepository<ProjectTaskLinked>
{
    Task<IEnumerable<ProjectTaskLinked>> GetLinkedTasksByTaskAsync(int taskId);
    Task<IEnumerable<ProjectTaskLinked>> GetLinkedTasksByRelationTypeAsync(string relationType);
}
