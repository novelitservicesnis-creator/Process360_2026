namespace Process360.Repository.ViewModel;

public class ProjectTaskDTO
{
    public int Id { get; set; }
    public int? ProjectTaskTypeId { get; set; }
    public int? SprintId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? PriorityId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public decimal? TotalTimeLogged { get; set; }
    public int? AssignTo { get; set; }
    public int? ReportedBy { get; set; }
    public int? CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; }
}
