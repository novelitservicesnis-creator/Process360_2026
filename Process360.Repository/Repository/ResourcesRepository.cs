using Microsoft.EntityFrameworkCore;
using Process360.Core;
using Process360.Core.Models;
using Process360.Repository.Interface;
using Process360.Repository.Repository.Base;

namespace Process360.Repository.Repository;

public class ResourcesRepository : RepositoryBase<Resources>, IResourcesRepository
{
    public ResourcesRepository(ProcessDbContext context) : base(context)
    {
    }

    public async Task<Resources?> GetResourceByEmailAsync(string email)
    {
        return await _dbSet.FirstOrDefaultAsync(r => r.Email == email);
    }

    public async Task<IEnumerable<Resources>> GetActiveResourcesAsync()
    {
        return await _dbSet.Where(r => r.IsActive == true).ToListAsync();
    }

    public async Task<IEnumerable<Resources>> GetResourcesByRoleAsync(string role)
    {
        return await _dbSet.Where(r => r.Role == role).ToListAsync();
    }
}
