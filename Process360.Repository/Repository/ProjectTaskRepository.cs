using Microsoft.EntityFrameworkCore;
using Process360.Core;
using Process360.Core.Models;
using Process360.Repository.Interface;
using Process360.Repository.Repository.Base;

namespace Process360.Repository.Repository;

public class ProjectTaskRepository : RepositoryBase<ProjectTask>, IProjectTaskRepository
{
    public ProjectTaskRepository(ProcessDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<ProjectTask>> GetTasksByProjectAsync(int projectId)
    {
        return await _dbSet.Include(pt => pt.ProjectTaskType)
            .Include(pt => pt.AssignedResource)
            .Include(pt => pt.ReportedByResource)
            .Where(pt => pt.Id == projectId)
            .ToListAsync();
    }

    public async Task<IEnumerable<ProjectTask>> GetTasksByAssigneeAsync(int resourceId)
    {
        return await _dbSet.Include(pt => pt.ProjectTaskType)
            .Include(pt => pt.AssignedResource)
            .Where(pt => pt.AssignTo == resourceId)
            .ToListAsync();
    }

    public async Task<IEnumerable<ProjectTask>> GetTasksByTypeAsync(int taskTypeId)
    {
        return await _dbSet.Include(pt => pt.ProjectTaskType)
            .Where(pt => pt.ProjectTaskTypeId == taskTypeId)
            .ToListAsync();
    }

    public async Task<IEnumerable<ProjectTask>> GetOverdueTasksAsync()
    {
        return await _dbSet.Include(pt => pt.ProjectTaskType)
            .Include(pt => pt.AssignedResource)
            .Where(pt => pt.EndDate < DateTime.Now && pt.EndDate != null)
            .ToListAsync();
    }

    public async Task<IEnumerable<ProjectTask>> GetTasksBySprint(int sprintId)
    {
        return await _dbSet.Include(pt => pt.ProjectTaskType)
            .Include(pt => pt.AssignedResource)
            .Where(pt => pt.SprintId == sprintId)
            .ToListAsync();
    }
}
