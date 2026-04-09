using Microsoft.EntityFrameworkCore;
using Process360.Core;
using Process360.Core.Models;
using Process360.Repository.Interface;
using Process360.Repository.Repository.Base;

namespace Process360.Repository.Repository;

public class ProjectPlanningRepository : RepositoryBase<ProjectPlanning>, IProjectPlanningRepository
{
    public ProjectPlanningRepository(ProcessDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<ProjectPlanning>> GetPlanningByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _dbSet.Where(pp => pp.StartDate >= startDate && pp.EndDate <= endDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<ProjectPlanning>> GetCurrentPlanningsAsync()
    {
        var today = DateTime.Now;
        return await _dbSet.Where(pp => pp.StartDate <= today && pp.EndDate >= today)
            .ToListAsync();
    }
}
