namespace Process360.Repository.ViewModel;

public class CreateProjectPlanningTasksDTO
{
    public int ProjectPlanningId { get; set; }
    public int ProjectTaskId { get; set; }
    public int? DurationHours { get; set; }
}
