using FoodTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodTracker.Infrastructure.Persistence.Configurations;

public class ReceiptConfiguration : IEntityTypeConfiguration<Receipt>
{
    public void Configure(EntityTypeBuilder<Receipt> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.ReceiptNumber)
            .HasMaxLength(100);

        builder.Property(x => x.TotalAmount)
            .HasPrecision(18, 2);

        builder.Property(x => x.Currency)
            .IsRequired()
            .HasMaxLength(3);

        builder.Property(x => x.OriginalFileName)
            .HasMaxLength(260);

        builder.Property(x => x.StoredFilePath)
            .HasMaxLength(500);

        builder.Property(x => x.FileHash)
            .HasMaxLength(128);

        builder.Property(x => x.SourceType)
            .HasMaxLength(50);

        builder.Property(x => x.DuplicateCheckStatus)
            .HasMaxLength(50);

        builder.Property(x => x.OcrStatus)
            .IsRequired();

        builder.Property(x => x.ParsingStatus)
            .IsRequired();

        builder.Property(x => x.NeedsReview)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .IsRequired();

        builder.HasIndex(x => x.FileHash);
        builder.HasIndex(x => x.StoreId);
        builder.HasIndex(x => x.PurchaseDate);
        builder.HasIndex(x => new { x.StoreId, x.PurchaseDate });

        builder.HasOne(x => x.Store)
            .WithMany(x => x.Receipts)
            .HasForeignKey(x => x.StoreId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(x => x.OcrLines)
            .WithOne(x => x.Receipt)
            .HasForeignKey(x => x.ReceiptId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.RawLines)
            .WithOne(x => x.Receipt)
            .HasForeignKey(x => x.ReceiptId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.PurchaseItems)
            .WithOne(x => x.Receipt)
            .HasForeignKey(x => x.ReceiptId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.ProcessingLogs)
            .WithOne(x => x.Receipt)
            .HasForeignKey(x => x.ReceiptId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
