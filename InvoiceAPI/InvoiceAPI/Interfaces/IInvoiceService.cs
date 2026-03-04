using InvoiceAPI.DTO;
using InvoiceAPI.Enums;
using InvoiceAPI.Models;

namespace InvoiceAPI.Interfaces;

public interface IInvoiceService {
    Task<InvoiceDto> CreateInvoiceAsync(CreateInvoiceDto invoice);
    Task<PagedResultDto<InvoiceListDto>> GetInvoicesAsync(InvoiceQueryDto query);
    Task<Invoice?> UpdateInvoiceAsync(Invoice invoice);
    Task<bool> ChangeInvoiceStatusAsync(int invoiceId, EInvoiceStatus status);
    Task<bool> HardDeleteInvoiceAsync(int invoiceId);
    Task<bool> SoftDeleteInvoiceAsync(int invoiceId);
    Task<Invoice?> GetInvoiceByIdAsync(int invoiceId);
    Task<bool> DownloadInvoiceAsPDF(int invoiceId);
}