namespace Process360.Repository.ViewModel;

public class ProjectTaskStatusHistoryDTO
{
    public int Id { get; set; }
    public int ProjectTaskId { get; set; }
    public int? OldStatusId { get; set; }
    public int? NewStatusId { get; set; }
    public int? CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; }
}
