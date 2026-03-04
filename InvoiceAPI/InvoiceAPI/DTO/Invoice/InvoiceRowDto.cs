namespace InvoiceAPI.DTO;

public class InvoiceRowDto {
    public string Service { get; set; } = null!;
    public decimal Quantity { get; set; }
    public decimal Rate { get; set; }
    public decimal Sum { get; set; }
}