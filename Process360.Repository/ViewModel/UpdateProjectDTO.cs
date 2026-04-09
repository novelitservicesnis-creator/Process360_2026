namespace Process360.Repository.ViewModel;

public class UpdateProjectDTO
{
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? DatabaseSchema { get; set; }
    public string? GitProvider { get; set; }
    public string? GitRepoUrl { get; set; }
    public bool? IsActive { get; set; }
}
