namespace InvoiceAPI.DTO;

public class CreateCustomerDto {
    public String Name { get; set; } = String.Empty;
    public String Email { get; set; } = String.Empty;
    public String? PhoneNumber { get; set; }
}