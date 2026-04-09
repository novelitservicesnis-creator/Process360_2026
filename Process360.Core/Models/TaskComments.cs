namespace Process360.Core.Models;

public class TaskComments
{
    public int Id { get; set; }
    public int ProjectTaskId { get; set; }
    public string? Comments { get; set; }
    public string? Attachment { get; set; }
    public int? CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; }

    // Navigation properties
    public virtual ProjectTask? ProjectTask { get; set; }
}
