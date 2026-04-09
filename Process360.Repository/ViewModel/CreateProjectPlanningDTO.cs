namespace Process360.Repository.ViewModel;

public class CreateProjectPlanningDTO
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Name { get; set; }
    public string? Goal { get; set; }
}
