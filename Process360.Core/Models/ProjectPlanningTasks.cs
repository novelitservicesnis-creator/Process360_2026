namespace Process360.Core.Models;

public class ProjectPlanningTasks
{
    public int Id { get; set; }
    public int? ProjectId { get; set; }
    public int? ProjectTaskId { get; set; }
    public bool? IsCompleted { get; set; }
    public int? CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; }

    // Navigation properties
    public virtual Project? Project { get; set; }
    public virtual ProjectTask? ProjectTask { get; set; }
    public virtual ProjectPlanning? ProjectPlanning { get; set; }
}
