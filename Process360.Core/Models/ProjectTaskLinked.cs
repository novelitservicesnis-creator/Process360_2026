namespace Process360.Core.Models;

public class ProjectTaskLinked
{
    public int Id { get; set; }
    public int ProjectTaskId { get; set; }
    public int RelatedProjectTaskId { get; set; }
    public string? RelationType { get; set; }
    public int? CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; }

    // Navigation properties
    public virtual ProjectTask? ProjectTask { get; set; }
    public virtual ProjectTask? RelatedProjectTask { get; set; }
}
