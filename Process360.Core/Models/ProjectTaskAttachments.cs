namespace Process360.Core.Models;

public class ProjectTaskAttachments
{
    public int Id { get; set; }
    public int ProjectTaskId { get; set; }
    public string? FileName { get; set; }
    public string? FileUrl { get; set; }
    public string? FileType { get; set; }
    public int? FileSize { get; set; }
    public int? CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; }

    // Navigation properties
    public virtual ProjectTask? ProjectTask { get; set; }
}
