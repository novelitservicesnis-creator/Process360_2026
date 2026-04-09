using Microsoft.EntityFrameworkCore;
using Process360.Core;
using Process360.Core.Models;
using Process360.Repository.Interface;
using Process360.Repository.Repository.Base;

namespace Process360.Repository.Repository;

public class ProjectRepository : RepositoryBase<Project>, IProjectRepository
{
    public ProjectRepository(ProcessDbContext context) : base(context)
    {
    }

    public async Task<Project?> GetProjectByCodeAsync(string code)
    {
        return await _dbSet.Include(p => p.Customer)
            .FirstOrDefaultAsync(p => p.Code == code);
    }

    public async Task<IEnumerable<Project>> GetProjectsByCustomerAsync(int customerId)
    {
        return await _dbSet.Include(p => p.Customer)
            .Where(p => p.CustomerID == customerId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Project>> GetActiveProjectsAsync()
    {
        return await _dbSet.Include(p => p.Customer)
            .Where(p => p.IsActive == true)
            .ToListAsync();
    }
}
