using FoodTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodTracker.Infrastructure.Persistence.Configurations;

public class StoreConfiguration : IEntityTypeConfiguration<Store>
{
    public void Configure(EntityTypeBuilder<Store> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Code)
            .IsRequired()
            .HasMaxLength(64);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.IsActive)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .IsRequired();

        builder.HasIndex(x => x.Code)
            .IsUnique();

        builder.HasMany(x => x.Receipts)
            .WithOne(x => x.Store)
            .HasForeignKey(x => x.StoreId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(x => x.Aliases)
            .WithOne(x => x.Store)
            .HasForeignKey(x => x.StoreId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
