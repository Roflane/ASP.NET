using InvoiceAPI.Db;
using InvoiceAPI.DTO;
using InvoiceAPI.Enums;
using InvoiceAPI.Interfaces;
using InvoiceAPI.Models;
using Microsoft.EntityFrameworkCore;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace InvoiceAPI.Services;

/// <summary>
/// Service for 'Invoice'
/// </summary>
/// <param name="ctx"></param>
public class InvoiceService(InvoiceAPIContext ctx) : IInvoiceService {
    public async Task<PagedResultDto<InvoiceListDto>> GetInvoicesAsync(InvoiceQueryDto query) {
        IQueryable<Invoice> invoices = ctx.Invoices
        .Include(i => i.Rows)
        .Include(i => i.Customer)
        .AsQueryable();

        // filtering
        if (!query.IncludeDeleted)
            invoices = invoices.Where(i => i.DeletedAt == null);

        if (query.CustomerId.HasValue)
            invoices = invoices.Where(i => i.CustomerId == query.CustomerId);

        if (query.Status.HasValue)
            invoices = invoices.Where(i => i.Status == query.Status);

        if (query.DateFrom.HasValue)
            invoices = invoices.Where(i => i.CreatedAt >= query.DateFrom);

        if (query.DateTo.HasValue)
            invoices = invoices.Where(i => i.CreatedAt <= query.DateTo);

        // sorting
        invoices = query.SortBy.ToLower() switch
        {
            "startdate" => query.SortDirection == "asc"
                ? invoices.OrderBy(i => i.StartDate)
                : invoices.OrderByDescending(i => i.StartDate),

            "enddate" => query.SortDirection == "asc"
                ? invoices.OrderBy(i => i.EndDate)
                : invoices.OrderByDescending(i => i.EndDate),

            "status" => query.SortDirection == "asc"
                ? invoices.OrderBy(i => i.Status)
                : invoices.OrderByDescending(i => i.Status),

            "updatedat" => query.SortDirection == "asc"
                ? invoices.OrderBy(i => i.UpdatedAt)
                : invoices.OrderByDescending(i => i.UpdatedAt),

            _ => query.SortDirection == "asc"
                ? invoices.OrderBy(i => i.CreatedAt)
                : invoices.OrderByDescending(i => i.CreatedAt),
        };

        var totalCount = await invoices.CountAsync();

        var items = await invoices
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(i => new InvoiceListDto // Маппинг прямо в запросе!
            {
                Id = i.Id,
                CustomerId = i.CustomerId,
                CustomerName = i.Customer.Name, // Предполагая, что есть Name
                StartDate = i.StartDate,
                EndDate = i.EndDate,
                Comment = i.Comment,
                TotalSum = i.Rows.Sum(r => r.Sum),
                Status = i.Status.ToString(),
                CreatedAt = i.CreatedAt,
                UpdatedAt = i.UpdatedAt,
                RowsCount = i.Rows.Count
            })
            .ToListAsync();

        return new PagedResultDto<InvoiceListDto>
        {
            Page = query.Page,
            PageSize = query.PageSize,
            TotalCount = totalCount,
            Items = items
        };
    }
    
    public async Task<InvoiceDto> CreateInvoiceAsync(CreateInvoiceDto dto) {
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
    
        return new InvoiceDto {
            Id = invoice.Id,
            CustomerId = invoice.CustomerId,
            Comment = invoice.Comment,
            TotalSum = invoice.Rows.Sum(r => r.Sum),
            Rows = invoice.Rows.Select(r => new InvoiceRowDto {
                Service = r.Service,
                Quantity = r.Quantity,
                Rate = r.Rate,
                Sum = r.Sum
            }).ToList()
        };
    }

    public async Task<Invoice?> UpdateInvoiceAsync(Invoice invoice) {
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

    public async Task<bool> ChangeInvoiceStatusAsync(int invoiceId, EInvoiceStatus status) {
        var invoice = await ctx.Invoices.FindAsync(invoiceId);
        if (invoice == null) return false;

        invoice.Status = status;
        invoice.UpdatedAt = DateTimeOffset.UtcNow;

        await ctx.SaveChangesAsync();
        return true;
    }

    public async Task<bool> HardDeleteInvoiceAsync(int invoiceId) {
        var invoice = await ctx.Invoices.FindAsync(invoiceId);
        if (invoice == null || invoice.Status != EInvoiceStatus.Created)
            return false;

        ctx.Invoices.Remove(invoice);
        await ctx.SaveChangesAsync();
        return true;
    }

    public async Task<bool> SoftDeleteInvoiceAsync(int invoiceId) {
        var invoice = await ctx.Invoices.FindAsync(invoiceId);
        if (invoice == null) return false;

        invoice.DeletedAt = DateTimeOffset.UtcNow;
        await ctx.SaveChangesAsync();
        return true;
    }

    public async Task<List<Invoice>> GetAllInvoicesAsync() {
        return await ctx.Invoices
            .Include(i => i.Rows)
            .Include(i => i.Customer)
            .ToListAsync();
    }

    public async Task<Invoice?> GetInvoiceByIdAsync(int invoiceId) {
        return await ctx.Invoices
            .Include(i => i.Rows)
            .Include(i => i.Customer)
            .FirstOrDefaultAsync(i => i.Id == invoiceId);
    }

    public async Task<bool> DownloadInvoiceAsPDF(int invoiceId) {
        var invoice = await GetInvoiceByIdAsync(invoiceId);
        
        using (var document = new PdfDocument()) {
            document.Info.Title = $"Invoice #{invoice.Id}";
            document.Info.Author = "Your Company";
            document.Info.Creator = "Invoice System";
            document.Info.Subject = $"Invoice for Customer #{invoice.CustomerId}";
            document.Info.Keywords = "invoice";
            
            PdfPage page = document.AddPage();
            page.Size = PdfSharp.PageSize.A4;
            
            using (XGraphics gfx = XGraphics.FromPdfPage(page)) {
                XFont titleFont = new XFont("Verdana", 20, XFontStyleEx.Bold);
                XFont headerFont = new XFont("Verdana", 12, XFontStyleEx.Bold);
                XFont regularFont = new XFont("Verdana", 10, XFontStyleEx.Regular);
                XFont boldFont = new XFont("Verdana", 10, XFontStyleEx.Bold);
                
                gfx.DrawString($"INVOICE #{invoice.Id}", titleFont, XBrushes.Black, 
                    new XRect(40, 40, page.Width, 50), XStringFormats.TopLeft);
                
                gfx.DrawString("Company Name", boldFont, XBrushes.Black, 40, 100);
                gfx.DrawString("67 Business Street", regularFont, XBrushes.Black, 40, 120);
                gfx.DrawString("City, State 12345", regularFont, XBrushes.Black, 40, 140);
                gfx.DrawString("Phone: (123) 456-7890", regularFont, XBrushes.Black, 40, 160);
                
                gfx.DrawString("BILL TO:", boldFont, XBrushes.Black, page.Width - 200, 100);
                gfx.DrawString($"Customer ID: {invoice.CustomerId}", regularFont, XBrushes.Black, page.Width - 200, 120);
                gfx.DrawString($"Date: {invoice.CreatedAt:MM/dd/yyyy}", regularFont, XBrushes.Black, page.Width - 200, 140);
                gfx.DrawString($"Status: {invoice.Status}", regularFont, XBrushes.Black, page.Width - 200, 160);
                
                gfx.DrawLine(XPens.Black, 40, 190, page.Width - 40, 190);
                
                gfx.DrawString("Service", boldFont, XBrushes.Black, 40, 220);
                gfx.DrawString("Qty", boldFont, XBrushes.Black, 300, 220);
                gfx.DrawString("Rate", boldFont, XBrushes.Black, 370, 220);
                gfx.DrawString("Sum", boldFont, XBrushes.Black, 440, 220);
                
                gfx.DrawLine(XPens.Black, 40, 225, page.Width - 40, 225);
                
                int yPos = 250;
                decimal total = 0;
                
                foreach (var row in invoice.Rows) {
                    gfx.DrawString(TruncateString(row.Service, 30), regularFont, XBrushes.Black, 40, yPos);
                    gfx.DrawString(row.Quantity.ToString("N2"), regularFont, XBrushes.Black, 300, yPos);
                    gfx.DrawString(row.Rate.ToString("C2"), regularFont, XBrushes.Black, 370, yPos);
                    gfx.DrawString(row.Sum.ToString("C2"), regularFont, XBrushes.Black, 440, yPos);
                    
                    total += row.Sum;
                    yPos += 25;
                    
                    if (yPos > page.Height - 100) {
                        page = document.AddPage();
                        gfx.Dispose();
                    }
                }
                
                gfx.DrawLine(XPens.Black, 40, yPos + 10, page.Width - 40, yPos + 10);
                
                gfx.DrawString($"SUBTOTAL:", boldFont, XBrushes.Black, 370, yPos + 35);
                gfx.DrawString(total.ToString("C2"), boldFont, XBrushes.Black, 440, yPos + 35);
                
                gfx.DrawString($"TOTAL:", titleFont, XBrushes.Black, 370, yPos + 70);
                gfx.DrawString(total.ToString("C2"), titleFont, XBrushes.Black, 440, yPos + 70);
                
                if (!string.IsNullOrEmpty(invoice.Comment)) {
                    gfx.DrawString("Comment:", boldFont, XBrushes.Black, 40, page.Height - 80);
                    gfx.DrawString(invoice.Comment, regularFont, XBrushes.Black, 40, page.Height - 60);
                }
                
                gfx.DrawString("Thank you for your business!", regularFont, XBrushes.Gray, 
                    page.Width / 2 - 80, page.Height - 30);
            }
            
            string fileName = $"Invoice_{invoice.Id}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName);
            
            document.Save(filePath);
        }
        
        return true;
    }
    
    private string TruncateString(string str, int maxLength) {
        if (string.IsNullOrEmpty(str)) return str;
        return str.Length <= maxLength ? str : str.Substring(0, maxLength - 3) + "...";
    }
}