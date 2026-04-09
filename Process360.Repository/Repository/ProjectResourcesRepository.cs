using Microsoft.EntityFrameworkCore;
using Process360.Core;
using Process360.Core.Models;
using Process360.Repository.Interface;
using Process360.Repository.Repository.Base;

namespace Process360.Repository.Repository;

public class ProjectResourcesRepository : RepositoryBase<ProjectResources>, IProjectResourcesRepository
{
    public ProjectResourcesRepository(ProcessDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<ProjectResources>> GetResourcesByProjectAsync(int projectId)
    {
        return await _dbSet.Include(pr => pr.Resource)
            .Include(pr => pr.Project)
            .Where(pr => pr.ProjectId == projectId)
            .ToListAsync();
    }

    public async Task<IEnumerable<ProjectResources>> GetProjectsByResourceAsync(int resourceId)
    {
        return await _dbSet.Include(pr => pr.Resource)
            .Include(pr => pr.Project)
            .Where(pr => pr.ResourceId == resourceId)
            .ToListAsync();
    }

    public async Task<IEnumerable<ProjectResources>> GetResourcesByRoleAsync(string role)
    {
        return await _dbSet.Include(pr => pr.Resource)
            .Include(pr => pr.Project)
            .Where(pr => pr.Role == role)
            .ToListAsync();
    }
}
