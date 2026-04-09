namespace Process360.Repository.ViewModel;

public class ProjectPlanningDTO
{
    public int Id { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Name { get; set; }
    public string? Goal { get; set; }
    public int? CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; }
}
