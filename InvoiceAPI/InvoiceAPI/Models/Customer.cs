namespace InvoiceAPI.Models;

public class Customer {
    public Int32 Id { get; set; }
    public String Name { get; set; } = String.Empty;
    public String? Address { get; set; }
    public String Email { get; set; } = String.Empty;
    public String? PhoneNumber { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? DeletedAt { get; set; }

    public List<Invoice> Invoices { get; set; } = new();
}