using FoodTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodTracker.Infrastructure.Persistence.Configurations;

public class ProductAliasConfiguration : IEntityTypeConfiguration<ProductAlias>
{
    public void Configure(EntityTypeBuilder<ProductAlias> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.RawText)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.NormalizedRawText)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.MatchMethod)
            .IsRequired();

        builder.Property(x => x.Confidence)
            .HasPrecision(5, 4);

        builder.Property(x => x.CreatedFromManualReview)
            .IsRequired();

        builder.Property(x => x.LastUsedAt);

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.HasIndex(x => x.NormalizedRawText);
        builder.HasIndex(x => x.ProductId);
        builder.HasIndex(x => x.StoreId);
        builder.HasIndex(x => new { x.StoreId, x.NormalizedRawText });

        builder.HasOne(x => x.Store)
            .WithMany(x => x.Aliases)
            .HasForeignKey(x => x.StoreId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(x => x.Product)
            .WithMany(x => x.Aliases)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
