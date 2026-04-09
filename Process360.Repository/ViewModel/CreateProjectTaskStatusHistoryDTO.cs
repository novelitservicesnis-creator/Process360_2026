namespace Process360.Repository.ViewModel;

public class CreateProjectTaskStatusHistoryDTO
{
    public int ProjectTaskId { get; set; }
    public string? Status { get; set; }
    public string? Reason { get; set; }
    public int? ChangedByResourceId { get; set; }
}
