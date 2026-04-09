namespace Process360.Repository.ViewModel;

public class CreateTaskCommentsDTO
{
    public int ProjectTaskId { get; set; }
    public string? Comment { get; set; }
    public int? CommentedByResourceId { get; set; }
}
