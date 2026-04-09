namespace Process360.Repository.ViewModel;

public class UpdateProjectTaskDTO
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public int? AssignTo { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int? ProjectTaskTypeId { get; set; }
    public decimal? TotalTimeLogged { get; set; }
}
