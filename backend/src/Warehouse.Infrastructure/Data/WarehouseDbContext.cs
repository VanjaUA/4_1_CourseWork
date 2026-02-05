using Microsoft.EntityFrameworkCore;
using Warehouse.Domain.Entities;
using Warehouse.Domain.Enums;

namespace Warehouse.Infrastructure.Data;

public class WarehouseDbContext : DbContext
{
    public WarehouseDbContext(DbContextOptions<WarehouseDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Warehouse.Domain.Entities.Warehouse> Warehouses { get; set; } // Explicit namespace to avoid conflict
    public DbSet<Partner> Partners { get; set; }
    public DbSet<Document> Documents { get; set; }
    public DbSet<DocumentItem> DocumentItems { get; set; }
    public DbSet<StockMovement> StockMovements { get; set; }
    public DbSet<Notification> Notifications { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User
        modelBuilder.Entity<User>().HasKey(u => u.Id);
        modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();

        // Product
        modelBuilder.Entity<Product>().HasKey(p => p.Id);
        modelBuilder.Entity<Product>().Property(p => p.UnitPrice).HasColumnType("decimal(18,2)");

        // Warehouse
        modelBuilder.Entity<Warehouse.Domain.Entities.Warehouse>().HasKey(w => w.Id);

        // Partner
        modelBuilder.Entity<Partner>().HasKey(p => p.Id);

        // Document
        modelBuilder.Entity<Document>().HasKey(d => d.Id);
        modelBuilder.Entity<Document>()
            .HasOne(d => d.Warehouse)
            .WithMany()
            .HasForeignKey(d => d.WarehouseId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Document>()
            .HasOne(d => d.Partner)
            .WithMany()
            .HasForeignKey(d => d.PartnerId)
            .OnDelete(DeleteBehavior.Restrict);
            
        modelBuilder.Entity<Document>()
            .HasOne(d => d.CreatedByUser)
            .WithMany()
            .HasForeignKey(d => d.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        // DocumentItem
        modelBuilder.Entity<DocumentItem>().HasKey(di => di.Id);
        modelBuilder.Entity<DocumentItem>()
            .HasOne(di => di.Document)
            .WithMany(d => d.Items)
            .HasForeignKey(di => di.DocumentId)
            .OnDelete(DeleteBehavior.Cascade);

        // StockMovement
        modelBuilder.Entity<StockMovement>().HasKey(sm => sm.Id);
        modelBuilder.Entity<StockMovement>().HasIndex(sm => sm.WarehouseId);
        modelBuilder.Entity<StockMovement>().HasIndex(sm => sm.ProductId);

        // Notification
        modelBuilder.Entity<Notification>().HasKey(n => n.Id);
    }
}
