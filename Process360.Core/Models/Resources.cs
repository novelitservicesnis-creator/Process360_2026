namespace Process360.Core.Models;

public class Resources
{
    public int Id { get; set; }
    public string Password { get; set; } = null!;
    public string? Role { get; set; }
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateOnly? DOB { get; set; }
    public string? Address { get; set; }
    public string? CurrentLocation { get; set; }
    public int? Experience { get; set; }
    public bool? IsActive { get; set; }
    public DateTime? CreatedDate { get; set; }

    // Navigation properties
    public virtual ICollection<ProjectTask> AssignedTasks { get; set; } = new List<ProjectTask>();
    public virtual ICollection<ProjectTask> ReportedTasks { get; set; } = new List<ProjectTask>();
    public virtual ICollection<ProjectResources> ProjectResources { get; set; } = new List<ProjectResources>();
}
