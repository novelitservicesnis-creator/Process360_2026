namespace Process360.Core.Models;

public class Technology
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Type { get; set; }
    public bool? IsActive { get; set; }
    public DateTime? CreatedDate { get; set; }
}
