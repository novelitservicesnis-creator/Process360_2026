using Microsoft.EntityFrameworkCore;
using Process360.Core;
using Process360.Core.Models;
using Process360.Repository.Interface;
using Process360.Repository.Repository.Base;

namespace Process360.Repository.Repository;

public class ProjectPlanningTasksRepository : RepositoryBase<ProjectPlanningTasks>, IProjectPlanningTasksRepository
{
    public ProjectPlanningTasksRepository(ProcessDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<ProjectPlanningTasks>> GetTasksByPlanningAsync(int planningId)
    {
        return await _dbSet.Include(ppt => ppt.Project)
            .Include(ppt => ppt.ProjectTask)
            .Where(ppt => ppt.Id == planningId)
            .ToListAsync();
    }

    public async Task<IEnumerable<ProjectPlanningTasks>> GetTasksByProjectAsync(int projectId)
    {
        return await _dbSet.Include(ppt => ppt.Project)
            .Include(ppt => ppt.ProjectTask)
            .Where(ppt => ppt.ProjectId == projectId)
            .ToListAsync();
    }

    public async Task<IEnumerable<ProjectPlanningTasks>> GetCompletedTasksAsync()
    {
        return await _dbSet.Include(ppt => ppt.Project)
            .Include(ppt => ppt.ProjectTask)
            .Where(ppt => ppt.IsCompleted == true)
            .ToListAsync();
    }
}
