namespace Process360.Repository.ViewModel;

public class CreateProjectTaskAttachmentsDTO
{
    public int ProjectTaskId { get; set; }
    public string? FileName { get; set; }
    public string? FileUrl { get; set; }
    public string? FileType { get; set; }
    public int? FileSize { get; set; }
}
