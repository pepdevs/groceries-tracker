using FoodTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodTracker.Infrastructure.Persistence.Configurations;

public class ProcessingLogConfiguration : IEntityTypeConfiguration<ProcessingLog>
{
    public void Configure(EntityTypeBuilder<ProcessingLog> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Step)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Status)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.Message)
            .HasMaxLength(2000);

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.HasOne(x => x.Receipt)
            .WithMany(x => x.ProcessingLogs)
            .HasForeignKey(x => x.ReceiptId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
