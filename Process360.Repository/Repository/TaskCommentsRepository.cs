using Microsoft.EntityFrameworkCore;
using Process360.Core;
using Process360.Core.Models;
using Process360.Repository.Interface;
using Process360.Repository.Repository.Base;

namespace Process360.Repository.Repository;

public class TaskCommentsRepository : RepositoryBase<TaskComments>, ITaskCommentsRepository
{
    public TaskCommentsRepository(ProcessDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<TaskComments>> GetCommentsByTaskAsync(int taskId)
    {
        return await _dbSet.Include(tc => tc.ProjectTask)
            .Where(tc => tc.ProjectTaskId == taskId)
            .ToListAsync();
    }

    public async Task<IEnumerable<TaskComments>> GetCommentsByUserAsync(int userId)
    {
        return await _dbSet.Include(tc => tc.ProjectTask)
            .Where(tc => tc.CreatedBy == userId)
            .ToListAsync();
    }
}
