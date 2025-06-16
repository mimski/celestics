using Celestics.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Celestics.Infrastructure.Configurations;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable("Transactions");
        builder.HasKey(t => t.Id);

        builder.Property(t => t.CreateDate)
            .IsRequired();

        builder.Property(t => t.Direction)
            .HasConversion<string>()
            .IsRequired()
            .HasMaxLength(6);

        builder.Property(t => t.Amount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(t => t.Currency)
            .IsRequired()
            .HasMaxLength(3);

        builder.Property(t => t.DebtorIban)
            .IsRequired()
            .HasMaxLength(34);

        builder.Property(t => t.BeneficiaryIban)
            .IsRequired()
            .HasMaxLength(34);

        builder.Property(t => t.Status)
            .HasConversion<string>()
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(t => t.ExternalId)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasOne(t => t.Merchant)
            .WithMany(m => m.Transactions)
            .HasForeignKey(t => t.MerchantId);

        builder.HasIndex(t => t.ExternalId)
            .IsUnique();

        builder.HasIndex(t => t.CreateDate);

        builder.HasIndex(t => t.Status);

        builder.HasIndex(t => t.DebtorIban);

        builder.HasIndex(t => t.BeneficiaryIban);
    }
}
