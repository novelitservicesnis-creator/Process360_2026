namespace Process360.Core.Models;

public class ProjectTaskStatusHistory
{
    public int Id { get; set; }
    public int ProjectTaskId { get; set; }
    public int? OldStatusId { get; set; }
    public int? NewStatusId { get; set; }
    public int? CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; }

    // Navigation properties
    public virtual ProjectTask? ProjectTask { get; set; }
}
