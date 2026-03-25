using FoodTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodTracker.Infrastructure.Persistence.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.NormalizedName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Subtype)
            .HasMaxLength(100);

        builder.Property(x => x.Brand)
            .HasMaxLength(100);

        builder.Property(x => x.SizeValue)
            .HasPrecision(18, 3);

        builder.Property(x => x.SizeUnit)
            .HasMaxLength(20);

        builder.Property(x => x.ComparableGroup)
            .HasMaxLength(100);

        builder.Property(x => x.Notes)
            .HasMaxLength(1000);

        builder.Property(x => x.IsActive)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .IsRequired();

        builder.HasIndex(x => x.NormalizedName);
        builder.HasIndex(x => x.CategoryId);
        builder.HasIndex(x => new { x.CategoryId, x.NormalizedName });

        builder.HasOne(x => x.Category)
            .WithMany(x => x.Products)
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Aliases)
            .WithOne(x => x.Product)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.PurchaseItems)
            .WithOne(x => x.Product)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
