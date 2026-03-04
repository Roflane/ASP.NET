namespace InvoiceAPI.DTO;

public class CustomerReportDto {
    public int CustomerId { get; set; }
    public int InvoiceCount { get; set; }
    public decimal TotalAmount { get; set; }
}
