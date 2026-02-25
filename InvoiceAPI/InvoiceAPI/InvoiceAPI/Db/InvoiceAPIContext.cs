using InvoiceAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InvoiceAPI.Db;

public class InvoiceAPIContext : IdentityDbContext<User, IdentityRole<Guid>, Guid> {
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Invoice> Invoices => Set<Invoice>();
    
    public InvoiceAPIContext(DbContextOptions<InvoiceAPIContext> options) 
        : base(options) { }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Customer>().HasQueryFilter(c => c.DeletedAt == null);
        modelBuilder.Entity<Invoice>().HasQueryFilter(i => i.DeletedAt == null);
    }
}