using Process360.Core.Models;

namespace Process360.Repository.Interface;

public interface ITaskCommentsRepository : IRepository<TaskComments>
{
    Task<IEnumerable<TaskComments>> GetCommentsByTaskAsync(int taskId);
    Task<IEnumerable<TaskComments>> GetCommentsByUserAsync(int userId);
}
