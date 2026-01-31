using InvoiceAPI.Db;
using InvoiceAPI.DTO;
using InvoiceAPI.Enums;
using InvoiceAPI.Interfaces;
using InvoiceAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace InvoiceAPI.Services;

public class CustomerService(InvoiceAPIContext ctx) : ICustomerService {
    public async Task<Customer> AddCustomerAsync(CreateCustomerDto dto) {
        var customer = new Customer {
            Name = dto.Name,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
        };
        await ctx.Customers.AddAsync(customer);
        await ctx.SaveChangesAsync();
        return customer;
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
