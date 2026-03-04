namespace InvoiceAPI.DTO;

public class ServiceReportDto {
    public string ServiceName { get; set; }
    public int InvoiceCount { get; set; }
    public decimal TotalSum { get; set; }
}