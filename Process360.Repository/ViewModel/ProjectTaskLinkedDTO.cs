namespace Process360.Repository.ViewModel;

public class ProjectTaskLinkedDTO
{
    public int Id { get; set; }
    public int ProjectTaskId { get; set; }
    public int RelatedProjectTaskId { get; set; }
    public string? RelationType { get; set; }
    public int? CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; }
}
