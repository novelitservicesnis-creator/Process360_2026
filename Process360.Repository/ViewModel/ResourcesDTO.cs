namespace Process360.Repository.ViewModel;

public class ResourcesDTO
{
    public int Id { get; set; }
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
}
