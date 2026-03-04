using InvoiceAPI.Enums;

namespace InvoiceAPI.DTO;

public class InvoiceQueryDto {
    // pagination
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;

    // filtering
    public int? CustomerId { get; set; }
    public EInvoiceStatus? Status { get; set; }
    public DateTimeOffset? DateFrom { get; set; }
    public DateTimeOffset? DateTo { get; set; }
    public bool IncludeDeleted { get; set; } = false;

    // sorting
    public string SortBy { get; set; } = "createdAt";
    public string SortDirection { get; set; } = "desc";
}
