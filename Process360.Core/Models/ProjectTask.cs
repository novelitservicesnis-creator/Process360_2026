namespace Process360.Core.Models;

public class ProjectTask
{
    public int Id { get; set; }
    public int? ProjectTaskTypeId { get; set; }
    public int? SprintId { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public int? PriorityId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public decimal? TotalTimeLogged { get; set; }
    public int? AssignTo { get; set; }
    public int? ReportedBy { get; set; }
    public int? CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; }

    // Navigation properties
    public virtual ProjectTaskType? ProjectTaskType { get; set; }
    public virtual Resources? AssignedResource { get; set; }
    public virtual Resources? ReportedByResource { get; set; }
    public virtual ICollection<ProjectTaskAttachments> ProjectTaskAttachments { get; set; } = new List<ProjectTaskAttachments>();
    public virtual ICollection<TaskComments> TaskComments { get; set; } = new List<TaskComments>();
    public virtual ICollection<ProjectTaskLinked> LinkedTasksFrom { get; set; } = new List<ProjectTaskLinked>();
    public virtual ICollection<ProjectTaskLinked> LinkedTasksTo { get; set; } = new List<ProjectTaskLinked>();
    public virtual ICollection<ProjectTaskStatusHistory> StatusHistories { get; set; } = new List<ProjectTaskStatusHistory>();
    public virtual ICollection<ProjectPlanningTasks> ProjectPlanningTasks { get; set; } = new List<ProjectPlanningTasks>();
}
