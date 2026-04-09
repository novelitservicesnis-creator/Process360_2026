namespace Process360.Repository.ViewModel;

public class ProjectDTO
{
    public int Id { get; set; }
    public int CustomerID { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? DatabaseSchema { get; set; }
    public string? GitProvider { get; set; }
    public string? GitRepoUrl { get; set; }
    public bool? IsActive { get; set; }
    public int? CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; }
}
