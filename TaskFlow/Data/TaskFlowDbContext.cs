using ASP_NET_08._TaskFlow_DTOs.Models;
using Microsoft.EntityFrameworkCore;

namespace ASP_NET_08._TaskFlow_DTOs.Data;

public class TaskFlowDbContext : DbContext
{
    public TaskFlowDbContext(DbContextOptions options) 
        : base(options)
    {}

    public DbSet<Project> Projects => Set<Project>();
    public DbSet<TaskItem> TaskItems => Set<TaskItem>();

    // Fluent API

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Project
        modelBuilder.Entity<Project>(
            p =>
            {
                p.HasKey(p => p.Id);
                p.Property(p => p.Name)
                    .IsRequired()
                    .HasMaxLength(200);
                p.Property(p => p.Description)
                    .HasMaxLength(1000);
                p.Property(p => p.CreatedAt)
                    .IsRequired();
            }
            );


        // TaskItem
        modelBuilder.Entity<TaskItem>(
            t =>
            {
                t.HasKey(t => t.Id);
                t.Property(t => t.Title)
                    .IsRequired()
                    .HasMaxLength(200);
                t.Property(t => t.Description)
                    .HasMaxLength(1000);
                t.Property(t => t.CreatedAt)
                    .IsRequired();
                t.Property(t => t.Status)
                   .IsRequired();

                t.HasOne(t => t.Project)
                    .WithMany(p => p.Tasks)
                    .HasForeignKey(t => t.ProjectId)
                    .OnDelete(DeleteBehavior.Cascade);
            }
            );
    }
}
