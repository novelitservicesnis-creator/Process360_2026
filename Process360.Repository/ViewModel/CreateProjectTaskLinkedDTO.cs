namespace Process360.Repository.ViewModel;

public class CreateProjectTaskLinkedDTO
{
    public int ProjectTaskId { get; set; }
    public int LinkedProjectTaskId { get; set; }
    public string? LinkType { get; set; }
}
