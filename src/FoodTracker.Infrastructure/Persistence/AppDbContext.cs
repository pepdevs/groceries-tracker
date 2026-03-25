using FoodTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FoodTracker.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Store> Stores { get; set; } = null!;
    public DbSet<Receipt> Receipts { get; set; } = null!;
    public DbSet<ReceiptOcrLine> ReceiptOcrLines { get; set; } = null!;
    public DbSet<ReceiptLineRaw> ReceiptLineRaws { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<ProductAlias> ProductAliases { get; set; } = null!;
    public DbSet<PurchaseItem> PurchaseItems { get; set; } = null!;
    public DbSet<ProcessingLog> ProcessingLogs { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
