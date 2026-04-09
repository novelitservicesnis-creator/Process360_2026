using Microsoft.EntityFrameworkCore;
using Process360.Core;
using Process360.Core.Models;
using Process360.Repository.Interface;
using Process360.Repository.Repository.Base;

namespace Process360.Repository.Repository;

public class ProjectTaskLinkedRepository : RepositoryBase<ProjectTaskLinked>, IProjectTaskLinkedRepository
{
    public ProjectTaskLinkedRepository(ProcessDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<ProjectTaskLinked>> GetLinkedTasksByTaskAsync(int taskId)
    {
        return await _dbSet.Include(ptl => ptl.ProjectTask)
            .Include(ptl => ptl.RelatedProjectTask)
            .Where(ptl => ptl.ProjectTaskId == taskId || ptl.RelatedProjectTaskId == taskId)
            .ToListAsync();
    }

    public async Task<IEnumerable<ProjectTaskLinked>> GetLinkedTasksByRelationTypeAsync(string relationType)
    {
        return await _dbSet.Include(ptl => ptl.ProjectTask)
            .Include(ptl => ptl.RelatedProjectTask)
            .Where(ptl => ptl.RelationType == relationType)
            .ToListAsync();
    }
}
