namespace Process360.Repository.ViewModel;

public class ProjectTaskAttachmentsDTO
{
    public int Id { get; set; }
    public int ProjectTaskId { get; set; }
    public string? FileName { get; set; }
    public string? FileUrl { get; set; }
    public string? FileType { get; set; }
    public int? FileSize { get; set; }
    public int? CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; }
}
