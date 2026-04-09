using Microsoft.EntityFrameworkCore;
using Process360.Core;
using Process360.Core.Models;
using Process360.Repository.Interface;
using Process360.Repository.Repository.Base;

namespace Process360.Repository.Repository;

public class TechnologyRepository : RepositoryBase<Technology>, ITechnologyRepository
{
    public TechnologyRepository(ProcessDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Technology>> GetTechnologiesByTypeAsync(string type)
    {
        return await _dbSet.Where(t => t.Type == type).ToListAsync();
    }

    public async Task<IEnumerable<Technology>> GetActiveTechnologiesAsync()
    {
        return await _dbSet.Where(t => t.IsActive == true).ToListAsync();
    }

    public async Task<Technology?> GetTechnologyByNameAsync(string name)
    {
        return await _dbSet.FirstOrDefaultAsync(t => t.Name == name);
    }
}
