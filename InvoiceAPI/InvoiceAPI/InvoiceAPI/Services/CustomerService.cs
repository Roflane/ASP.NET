using InvoiceAPI.Db;
using InvoiceAPI.DTO;
using InvoiceAPI.Enums;
using InvoiceAPI.Interfaces;
using InvoiceAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace InvoiceAPI.Services;

/// <summary>
/// Service for 'Customer'
/// </summary>
/// <param name="ctx"></param>
public class CustomerService(InvoiceAPIContext ctx) : ICustomerService {
    public async Task<Customer> CreateCustomerAsync(CreateCustomerDto dto) {
        var customer = new Customer {
            Name = dto.Name,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
        };
        await ctx.Customers.AddAsync(customer);
        await ctx.SaveChangesAsync();
        return customer;
    }
    
    public async Task<PagedResult<Customer>> GetCustomersAsync(CustomerQueryDto query)
    {
        IQueryable<Customer> customers = ctx.Customers
            .Include(c => c.Invoices)
            .AsQueryable();

        // filtering
        if (!query.IncludeDeleted)
            customers = customers.Where(c => c.DeletedAt == null);

        if (!string.IsNullOrWhiteSpace(query.Name))
            customers = customers.Where(c =>
                c.Name.ToLower().Contains(query.Name.ToLower()));

        if (!string.IsNullOrWhiteSpace(query.Email))
            customers = customers.Where(c =>
                c.Email.ToLower().Contains(query.Email.ToLower()));

        // sorting
        customers = query.SortBy.ToLower() switch
        {
            "name" => query.SortDirection == "asc"
                ? customers.OrderBy(c => c.Name)
                : customers.OrderByDescending(c => c.Name),

            "email" => query.SortDirection == "asc"
                ? customers.OrderBy(c => c.Email)
                : customers.OrderByDescending(c => c.Email),

            "updatedat" => query.SortDirection == "asc"
                ? customers.OrderBy(c => c.UpdatedAt)
                : customers.OrderByDescending(c => c.UpdatedAt),

            _ => query.SortDirection == "asc"
                ? customers.OrderBy(c => c.CreatedAt)
                : customers.OrderByDescending(c => c.CreatedAt),
        };

        var totalCount = await customers.CountAsync();

        var items = await customers
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync();

        return new PagedResult<Customer>
        {
            Page = query.Page,
            PageSize = query.PageSize,
            TotalCount = totalCount,
            Items = items
        };
    }
    

    public async Task<Customer?> UpdateCustomerAsync(Customer customer)
    {
        var existing = await ctx.Customers.FindAsync(customer.Id);
        if (existing == null) return null;

        existing.Name = customer.Name;
        existing.Address = customer.Address;
        existing.Email = customer.Email;
        existing.PhoneNumber = customer.PhoneNumber;
        existing.UpdatedAt = DateTimeOffset.UtcNow;

        await ctx.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> SoftDeleteCustomerAsync(int customerId)
    {
        var customer = await ctx.Customers.Include(c => c.Invoices)
            .FirstOrDefaultAsync(c => c.Id == customerId);
        if (customer == null) return false;

        customer.DeletedAt = DateTimeOffset.UtcNow;
        await ctx.SaveChangesAsync();
        return true;
    }

    public async Task<bool> HardDeleteCustomerAsync(int customerId)
    {
        var customer = await ctx.Customers.Include(c => c.Invoices)
            .FirstOrDefaultAsync(c => c.Id == customerId);
        if (customer == null) return false;

        if (customer.Invoices.Any(i => i.Status != EInvoiceStatus.Created))
            return false;

        ctx.Customers.Remove(customer);
        await ctx.SaveChangesAsync();
        return true;
    }
    public async Task<List<Customer>> GetAllCustomersAsync() =>
        await ctx.Customers.Include(c => c.Invoices).ToListAsync();

    public async Task<Customer?> GetCustomerByIdAsync(int customerId) =>
        await ctx.Customers.Include(c => c.Invoices)
            .FirstOrDefaultAsync(c => c.Id == customerId);
}
