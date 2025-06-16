using Celestics.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Celestics.Infrastructure.Configurations;

public class MerchantConfiguration : IEntityTypeConfiguration<Merchant>
{
    public void Configure(EntityTypeBuilder<Merchant> builder)
    {
        builder.ToTable("Merchants");
        builder.HasKey(m => m.Id);

        builder.Property(m => m.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(m => m.BoardingDate)
            .IsRequired();

        builder.Property(m => m.Url)
            .HasMaxLength(500);

        builder.Property(m => m.Country)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(m => m.Address1)
            .IsRequired()
            .HasMaxLength(300);

        builder.Property(m => m.Address2)
            .HasMaxLength(300);

        builder.HasOne(m => m.Partner)
            .WithMany(p => p.Merchants)
            .HasForeignKey(m => m.PartnerId);

        builder.HasMany(m => m.Transactions)
            .WithOne(t => t.Merchant)
            .HasForeignKey(t => t.MerchantId);
    }
}
