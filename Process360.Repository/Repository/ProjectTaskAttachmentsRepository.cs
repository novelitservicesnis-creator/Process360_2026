using Microsoft.EntityFrameworkCore;
using Process360.Core;
using Process360.Core.Models;
using Process360.Repository.Interface;
using Process360.Repository.Repository.Base;

namespace Process360.Repository.Repository;

public class ProjectTaskAttachmentsRepository : RepositoryBase<ProjectTaskAttachments>, IProjectTaskAttachmentsRepository
{
    public ProjectTaskAttachmentsRepository(ProcessDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<ProjectTaskAttachments>> GetAttachmentsByTaskAsync(int taskId)
    {
        return await _dbSet.Include(pta => pta.ProjectTask)
            .Where(pta => pta.ProjectTaskId == taskId)
            .ToListAsync();
    }
}
