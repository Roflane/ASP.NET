using InvoiceAPI.Db;
using InvoiceAPI.DTO;
using InvoiceAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InvoiceAPI.Services;

public class InvoiceReportService(InvoiceAPIContext ctx) : IInvoiceReportService {
    public async Task<List<CustomerReportDto>> GetCustomerReportAsync(DateTime startDate, DateTime endDate) {
        return await ctx.Invoices
            .Where(i => i.CreatedAt >= startDate && i.CreatedAt <= endDate)
            .GroupBy(i => new { i.CustomerId, i.Customer.Name })
            .Select(g => new CustomerReportDto {
                CustomerId = g.Key.CustomerId,
                InvoiceCount = g.Count(),
                TotalAmount = g.Sum(i => i.TotalSum)
            })
            .OrderByDescending(r => r.TotalAmount)
            .ToListAsync();
    }

    public async Task<List<ServiceReportDto>> GetServiceReportAsync(DateTime startDate, DateTime endDate) {
        return await ctx.Invoices
            .Include(i => i.Rows)
            .ThenInclude(r => r.Service)
            .Where(i => i.StartDate >= startDate && i.EndDate <= endDate)
            .SelectMany(i => i.Rows, (invoice, row) => new { invoice, row })
            .GroupBy(x => new { x.row.Id, x.row.Service })
            .Select(g => new ServiceReportDto {
                ServiceName = g.Key.Service,
                InvoiceCount = g.Select(x => x.invoice.Id).Distinct().Count(),
                TotalSum = g.Sum(x => x.row.Sum) // или x.row.Sum если есть такое поле
            })
            .OrderByDescending(r => r.InvoiceCount)
            .ToListAsync();
    }

    public async Task<List<InvoiceReportDto>> GetInvoiceStatusReportAsync(DateTime startDate, DateTime endDate) {
        return await ctx.Invoices
            .Where(i => i.CreatedAt >= startDate && i.CreatedAt <= endDate)
            .GroupBy(i => i.Status)
            .Select(g => new InvoiceReportDto {
                Status = g.Key,
                InvoiceCount = g.Count(),
                TotalAmount = g.Sum(i => i.TotalSum)
            })
            .ToListAsync();
    }
}