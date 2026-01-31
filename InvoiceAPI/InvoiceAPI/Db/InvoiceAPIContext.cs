using InvoiceAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace InvoiceAPI.Db;

public class InvoiceAPIContext(DbContextOptions options) : DbContext(options) {
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Invoice> Invoices => Set<Invoice>();
    public DbSet<InvoiceRow> InvoiceRows => Set<InvoiceRow>();

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Customer>().HasQueryFilter(c => c.DeletedAt == null);
        modelBuilder.Entity<Invoice>().HasQueryFilter(i => i.DeletedAt == null);
    }
}