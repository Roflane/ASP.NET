using InvoiceAPI.Enums;

namespace InvoiceAPI.DTO;

public class InvoiceReportDto {
    public EInvoiceStatus Status { get; set; }
    public int InvoiceCount { get; set; }
    public decimal TotalAmount { get; set; }
}