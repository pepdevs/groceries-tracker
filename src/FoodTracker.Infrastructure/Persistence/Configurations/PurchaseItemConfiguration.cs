using FoodTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodTracker.Infrastructure.Persistence.Configurations;

public class PurchaseItemConfiguration : IEntityTypeConfiguration<PurchaseItem>
{
    public void Configure(EntityTypeBuilder<PurchaseItem> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.PurchaseDate)
            .IsRequired();

        builder.Property(x => x.RawName)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.Quantity)
            .HasPrecision(18, 3);

        builder.Property(x => x.Unit)
            .HasMaxLength(50);

        builder.Property(x => x.UnitPrice)
            .HasPrecision(18, 2);

        builder.Property(x => x.LineTotal)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(x => x.DiscountAmount)
            .HasPrecision(18, 2);

        builder.Property(x => x.WasAutoMatched)
            .IsRequired();

        builder.Property(x => x.ReviewStatus)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .IsRequired();

        builder.HasIndex(x => x.PurchaseDate);
        builder.HasIndex(x => x.ProductId);
        builder.HasIndex(x => x.StoreId);
        builder.HasIndex(x => x.ReceiptId);
        builder.HasIndex(x => new { x.ProductId, x.PurchaseDate });
        builder.HasIndex(x => new { x.StoreId, x.PurchaseDate });

        builder.HasOne(x => x.Receipt)
            .WithMany(x => x.PurchaseItems)
            .HasForeignKey(x => x.ReceiptId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.ReceiptLineRaw)
            .WithMany()
            .HasForeignKey(x => x.ReceiptLineRawId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Product)
            .WithMany(x => x.PurchaseItems)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Store)
            .WithMany()
            .HasForeignKey(x => x.StoreId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
