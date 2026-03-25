using FoodTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodTracker.Infrastructure.Persistence.Configurations;

public class ReceiptOcrLineConfiguration : IEntityTypeConfiguration<ReceiptOcrLine>
{
    public void Configure(EntityTypeBuilder<ReceiptOcrLine> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.LineNumber)
            .IsRequired();

        builder.Property(x => x.Text)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(x => x.Confidence)
            .HasPrecision(5, 4);

        builder.Property(x => x.BoundingBoxJson)
            .HasMaxLength(4000);

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.HasIndex(x => new { x.ReceiptId, x.LineNumber });

        builder.HasOne(x => x.Receipt)
            .WithMany(x => x.OcrLines)
            .HasForeignKey(x => x.ReceiptId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
