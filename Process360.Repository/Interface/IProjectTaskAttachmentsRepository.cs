using Process360.Core.Models;

namespace Process360.Repository.Interface;

public interface IProjectTaskAttachmentsRepository : IRepository<ProjectTaskAttachments>
{
    Task<IEnumerable<ProjectTaskAttachments>> GetAttachmentsByTaskAsync(int taskId);
}
