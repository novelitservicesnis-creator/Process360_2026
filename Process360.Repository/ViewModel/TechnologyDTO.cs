namespace Process360.Repository.ViewModel;

public class TechnologyDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Type { get; set; }
    public bool? IsActive { get; set; }
    public DateTime? CreatedDate { get; set; }
}
