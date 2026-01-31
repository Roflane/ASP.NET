using InvoiceAPI.Models;

namespace InvoiceAPI.DTO;

public class CreateInvoiceDto {
    public Int32 CustomerId { get; set; }   
    public Decimal TotalSum { get; set; }
    public String? Comment { get; set; }
    public List<InvoiceRow> Rows { get; set; } = new();
}