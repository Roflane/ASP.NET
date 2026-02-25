using InvoiceAPI.DTO;
using InvoiceAPI.Enums;
using InvoiceAPI.Models;

namespace InvoiceAPI.Interfaces;

public interface IInvoiceService {
    Task<Invoice> CreateInvoiceAsync(CreateInvoiceDto invoice);
    Task<PagedResult<Invoice>> GetInvoicesAsync(InvoiceQueryDto query);
    Task<Invoice?> UpdateInvoiceAsync(Invoice invoice);
    Task<bool> ChangeInvoiceStatusAsync(int invoiceId, EInvoiceStatus status);
    Task<bool> HardDeleteInvoiceAsync(int invoiceId);
    Task<bool> SoftDeleteInvoiceAsync(int invoiceId);
    Task<List<Invoice>> GetAllInvoicesAsync();
    Task<Invoice?> GetInvoiceByIdAsync(int invoiceId);
}