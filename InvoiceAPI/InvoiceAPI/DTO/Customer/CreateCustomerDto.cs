namespace InvoiceAPI.DTO;

public class CreateCustomerDto {
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
}