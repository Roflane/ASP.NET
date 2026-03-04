namespace InvoiceAPI.DTO;

public class InvoiceListDto {
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } 
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
    public string? Comment { get; set; }
    public decimal TotalSum { get; set; }
    public string Status { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public int RowsCount { get; set; } 
}