namespace Process360.Repository.ViewModel;

/// <summary>
/// DTO for updating an existing customer
/// </summary>
public class UpdateCustomerDTO
{
    public int Id { get; set; }
    public string? Login { get; set; }
    public string? Name { get; set; }
    public string? Website { get; set; }
    public string? Email { get; set; }
    public string? Company { get; set; }
    public bool? IsActive { get; set; }
}
