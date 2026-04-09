namespace Process360.Core.Models;

public class Customer
{
    public int Id { get; set; }
    public string Login { get; set; } = null!;
    public string? Website { get; set; }
    public string? Email { get; set; }
    public string Name { get; set; } = null!;
    public string? Company { get; set; }
    public bool? IsActive { get; set; }
    public int? CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; }

    // Navigation properties
    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();
}
