namespace Process360.Repository.ViewModel;

public class TaskCommentsDTO
{
    public int Id { get; set; }
    public int ProjectTaskId { get; set; }
    public string? Comments { get; set; }
    public string? Attachment { get; set; }
    public int? CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; }
}
