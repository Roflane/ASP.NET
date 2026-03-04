namespace InvoiceAPI.DTO;

public class CreateInvoiceRowDto {
    public string Service { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal Rate { get; set; }
}