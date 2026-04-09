namespace Process360.Repository.ViewModel;

public class CustomerDTO
{
    public int Id { get; set; }
    public string Login { get; set; } = string.Empty;
    public string? Website { get; set; }
    public string? Email { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Company { get; set; }
    public bool? IsActive { get; set; }
    public int? CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; }
}
