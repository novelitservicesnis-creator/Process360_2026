namespace Process360.Core.Models;

public class ProjectPlanning
{
    public int Id { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Name { get; set; }
    public string? Goal { get; set; }
    public int? CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; }

    // Navigation properties
    public virtual ICollection<ProjectPlanningTasks> ProjectPlanningTasks { get; set; } = new List<ProjectPlanningTasks>();
}
