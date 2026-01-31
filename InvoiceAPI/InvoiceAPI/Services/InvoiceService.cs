using InvoiceAPI.Db;
using InvoiceAPI.DTO;
using InvoiceAPI.Enums;
using InvoiceAPI.Interfaces;
using InvoiceAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace InvoiceAPI.Services;

public class InvoiceService(InvoiceAPIContext ctx) : IInvoiceService {
    public async Task<Invoice> CreateInvoiceAsync(CreateInvoiceDto dto) {
        var invoice = new Invoice {
            CustomerId = dto.CustomerId,
            Comment = dto.Comment,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow,
            Status = EInvoiceStatus.Created,
            Rows = dto.Rows.Select(r => new InvoiceRow {
                Service = r.Service,
                Quantity = r.Quantity,
                Rate = r.Rate,
                Sum = r.Quantity * r.Rate
            }).ToList()
        };
        await ctx.Invoices.AddAsync(invoice);
        await ctx.SaveChangesAsync();
        return invoice;
    }

    public async Task<Invoice?> UpdateInvoiceAsync(Invoice invoice)
    {
        var existing = await ctx.Invoices.Include(i => i.Rows)
            .FirstOrDefaultAsync(i => i.Id == invoice.Id);

        if (existing == null || existing.Status != EInvoiceStatus.Created)
            return null; 

        existing.StartDate = invoice.StartDate;
        existing.EndDate = invoice.EndDate;
        existing.Comment = invoice.Comment;
        existing.UpdatedAt = DateTimeOffset.UtcNow;

        existing.Rows.Clear();
        foreach (var row in invoice.Rows)
        {
            row.Sum = row.Quantity * row.Rate;
            existing.Rows.Add(row);
        }

        await ctx.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> ChangeInvoiceStatusAsync(int invoiceId, EInvoiceStatus status)
    {
        var invoice = await ctx.Invoices.FindAsync(invoiceId);
        if (invoice == null) return false;

        invoice.Status = status;
        invoice.UpdatedAt = DateTimeOffset.UtcNow;

        await ctx.SaveChangesAsync();
        return true;
    }

    public async Task<bool> HardDeleteInvoiceAsync(int invoiceId)
    {
        var invoice = await ctx.Invoices.FindAsync(invoiceId);
        if (invoice == null || invoice.Status != EInvoiceStatus.Created)
            return false;

        ctx.Invoices.Remove(invoice);
        await ctx.SaveChangesAsync();
        return true;
    }

    public async Task<bool> SoftDeleteInvoiceAsync(int invoiceId)
    {
        var invoice = await ctx.Invoices.FindAsync(invoiceId);
        if (invoice == null) return false;

        invoice.DeletedAt = DateTimeOffset.UtcNow;
        await ctx.SaveChangesAsync();
        return true;
    }

    public async Task<List<Invoice>> GetAllInvoicesAsync()
    {
        return await ctx.Invoices
            .Include(i => i.Rows)
            .Include(i => i.Customer)
            .ToListAsync();
    }

    public async Task<Invoice?> GetInvoiceByIdAsync(int invoiceId)
    {
        return await ctx.Invoices
            .Include(i => i.Rows)
            .Include(i => i.Customer)
            .FirstOrDefaultAsync(i => i.Id == invoiceId);
    }
}