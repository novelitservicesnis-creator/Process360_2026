namespace Process360.Repository.ViewModel;

public class ProjectPlanningTasksDTO
{
    public int Id { get; set; }
    public int? ProjectId { get; set; }
    public int? ProjectTaskId { get; set; }
    public bool? IsCompleted { get; set; }
    public int? CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; }
}
