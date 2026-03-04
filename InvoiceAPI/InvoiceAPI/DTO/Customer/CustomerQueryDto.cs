namespace InvoiceAPI.DTO;

public class CustomerQueryDto {
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;

    public string? Name { get; set; }
    public string? Email { get; set; }
    public bool IncludeDeleted { get; set; } = false;

    public string SortBy { get; set; } = "createdAt";
    public string SortDirection { get; set; } = "desc";
}
