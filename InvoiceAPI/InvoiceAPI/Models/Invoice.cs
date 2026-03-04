using System.ComponentModel.DataAnnotations.Schema;
using InvoiceAPI.Enums;

namespace InvoiceAPI.Models;

public class Invoice {
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
    public List<InvoiceRow> Rows { get; set; } = new();
    
    [NotMapped] public Decimal TotalSum => Rows?.Sum(r => r.Sum) ?? 0;
    public string? Comment { get; set; }
    public EInvoiceStatus Status { get; set; } = EInvoiceStatus.Created;
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? DeletedAt { get; set; }
}