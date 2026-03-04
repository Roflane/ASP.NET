using InvoiceAPI.DTO;

namespace InvoiceAPI.Interfaces;

public interface IInvoiceReportService {
    Task<List<CustomerReportDto>> GetCustomerReportAsync(DateTime startDate, DateTime endDate);
    Task<List<ServiceReportDto>> GetServiceReportAsync(DateTime startDate, DateTime endDate);
    Task<List<InvoiceReportDto>> GetInvoiceStatusReportAsync(DateTime startDate, DateTime endDate);
}