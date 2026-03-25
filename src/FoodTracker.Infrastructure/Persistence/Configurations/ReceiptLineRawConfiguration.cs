using FoodTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodTracker.Infrastructure.Persistence.Configurations;

public class ReceiptLineRawConfiguration : IEntityTypeConfiguration<ReceiptLineRaw>
{
    public void Configure(EntityTypeBuilder<ReceiptLineRaw> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.RawText)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(x => x.DetectedPrice)
            .HasPrecision(18, 2);

        builder.Property(x => x.DetectedQuantity)
            .HasPrecision(18, 3);

        builder.Property(x => x.DetectedUnit)
            .HasMaxLength(50);

        builder.Property(x => x.IsProductLine)
            .IsRequired();

        builder.Property(x => x.IgnoredReason)
            .HasMaxLength(200);

        builder.Property(x => x.ParseConfidence)
            .HasPrecision(5, 4);

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.HasIndex(x => x.ReceiptId);
        builder.HasIndex(x => x.SourceOcrLineNumber);
        builder.HasIndex(x => new { x.ReceiptId, x.SourceOcrLineNumber });

        builder.HasOne(x => x.Receipt)
            .WithMany(x => x.RawLines)
            .HasForeignKey(x => x.ReceiptId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
