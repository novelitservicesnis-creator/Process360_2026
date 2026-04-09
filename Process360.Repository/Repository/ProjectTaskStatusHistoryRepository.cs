using Microsoft.EntityFrameworkCore;
using Process360.Core;
using Process360.Core.Models;
using Process360.Repository.Interface;
using Process360.Repository.Repository.Base;

namespace Process360.Repository.Repository;

public class ProjectTaskStatusHistoryRepository : RepositoryBase<ProjectTaskStatusHistory>, IProjectTaskStatusHistoryRepository
{
    public ProjectTaskStatusHistoryRepository(ProcessDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<ProjectTaskStatusHistory>> GetStatusHistoryByTaskAsync(int taskId)
    {
        return await _dbSet.Include(ptsh => ptsh.ProjectTask)
            .Where(ptsh => ptsh.ProjectTaskId == taskId)
            .ToListAsync();
    }
}
