using Microsoft.AspNetCore.Identity;

namespace InvoiceAPI.Models;

public class User : IdentityUser<Guid>
{
    public string Name { get; set; } = string.Empty;
    public string? Address { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
    public ICollection<Customer> Customers { get; set; } = new List<Customer>();
}