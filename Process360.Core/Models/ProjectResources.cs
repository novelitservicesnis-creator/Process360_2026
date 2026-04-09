namespace Process360.Core.Models;

public class ProjectResources
{
    public int Id { get; set; }
    public int ResourceId { get; set; }
    public int ProjectId { get; set; }
    public string? Role { get; set; }

    // Navigation properties
    public virtual Resources? Resource { get; set; }
    public virtual Project? Project { get; set; }
}
