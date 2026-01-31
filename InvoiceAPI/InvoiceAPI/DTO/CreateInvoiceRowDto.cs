namespace InvoiceAPI.DTO;

public class CreateInvoiceRowDto {
    public String Service { get; set; } = String.Empty;
    public Int32 Quantity { get; set; }
    public Decimal Rate { get; set; }
}