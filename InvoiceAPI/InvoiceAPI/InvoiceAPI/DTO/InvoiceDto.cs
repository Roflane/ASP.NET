namespace InvoiceAPI.DTO;

public class InvoiceDto {
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public string? Comment { get; set; }
    public decimal TotalSum { get; set; }
    public List<InvoiceRowDto> Rows { get; set; } = [];
}