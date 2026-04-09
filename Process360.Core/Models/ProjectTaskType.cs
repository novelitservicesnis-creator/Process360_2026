namespace Process360.Core.Models;

public class ProjectTaskType
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int? CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; }

    // Navigation properties
    public virtual ICollection<ProjectTask> ProjectTasks { get; set; } = new List<ProjectTask>();
}
