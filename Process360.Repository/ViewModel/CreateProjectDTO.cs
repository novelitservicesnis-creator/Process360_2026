namespace Process360.Repository.ViewModel;

public class CreateProjectDTO
{
    public string? Code { get; set; }
    public string? Name { get; set; }
    public int? CustomerId { get; set; }
    public string? DatabaseSchema { get; set; }
    public string? GitProvider { get; set; }
    public string? GitRepoUrl { get; set; }
}
