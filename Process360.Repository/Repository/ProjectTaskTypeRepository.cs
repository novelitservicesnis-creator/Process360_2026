using Microsoft.EntityFrameworkCore;
using Process360.Core;
using Process360.Core.Models;
using Process360.Repository.Interface;
using Process360.Repository.Repository.Base;

namespace Process360.Repository.Repository;

public class ProjectTaskTypeRepository : RepositoryBase<ProjectTaskType>, IProjectTaskTypeRepository
{
    public ProjectTaskTypeRepository(ProcessDbContext context) : base(context)
    {
    }

    public async Task<ProjectTaskType?> GetTaskTypeByNameAsync(string name)
    {
        return await _dbSet.FirstOrDefaultAsync(pt => pt.Name == name);
    }
}
