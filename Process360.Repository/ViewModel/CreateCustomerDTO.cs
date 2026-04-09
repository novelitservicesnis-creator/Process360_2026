namespace Process360.Repository.ViewModel;

/// <summary>
/// DTO for creating a new customer
/// </summary>
public class CreateCustomerDTO
{
    public string Login { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Website { get; set; }
    public string? Email { get; set; }
    public string? Company { get; set; }
}
