namespace Process360.Core.Models;

public class Project
{
    public int Id { get; set; }
    public int CustomerID { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? DatabaseSchema { get; set; }
    public string? GitProvider { get; set; }
    public string? GitRepoUrl { get; set; }
    public byte[]? GitAccessToken { get; set; }
    public bool? IsActive { get; set; }
    public int? CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; }

    // Navigation properties
    public virtual Customer? Customer { get; set; }
    public virtual ICollection<ProjectTask> ProjectTasks { get; set; } = new List<ProjectTask>();
    public virtual ICollection<ProjectResources> ProjectResources { get; set; } = new List<ProjectResources>();
    public virtual ICollection<ProjectPlanning> ProjectPlannings { get; set; } = new List<ProjectPlanning>();
    public virtual ICollection<ProjectPlanningTasks> ProjectPlanningTasks { get; set; } = new List<ProjectPlanningTasks>();
}
