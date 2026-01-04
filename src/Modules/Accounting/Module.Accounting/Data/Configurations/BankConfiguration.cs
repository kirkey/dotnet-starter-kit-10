using FSH.Module.Accounting.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Module.Accounting.Data.Configurations;

public class BankConfiguration : IEntityTypeConfiguration<Bank>
{
    public void Configure(EntityTypeBuilder<Bank> builder)
    {
        builder.ToTable("Banks", "accounting");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.BankName).IsRequired().HasMaxLength(AccountingStringLengths.BankName);
        builder.Property(x => x.BankCode).HasMaxLength(AccountingStringLengths.Medium);
        builder.Property(x => x.Address).HasMaxLength(AccountingStringLengths.XHuge);
        builder.Property(x => x.ContactName).HasMaxLength(AccountingStringLengths.Name);
        builder.Property(x => x.ContactPhone).HasMaxLength(AccountingStringLengths.Medium);
        builder.Property(x => x.RoutingNumber).HasMaxLength(AccountingStringLengths.Medium);
        builder.Property(x => x.SwiftCode).HasMaxLength(AccountingStringLengths.Medium);
        builder.Property(x => x.CurrencyCode).HasMaxLength(AccountingStringLengths.Small);

        builder.Property(x => x.OpeningBalance).IsRequired().HasPrecision(18, 2);
        builder.Property(x => x.CurrentBalance).IsRequired().HasPrecision(18, 2);
        builder.Property(x => x.IsDefault).IsRequired();

        builder.Property(x => x.Description).HasMaxLength(AccountingStringLengths.Description);
        builder.Property(x => x.IsActive).IsRequired();
        builder.Property(x => x.TenantId).IsRequired().HasMaxLength(AccountingStringLengths.TenantId);

        builder.HasIndex(x => x.TenantId);
        builder.HasIndex(x => new { x.TenantId, x.BankName }).IsUnique();
        builder.HasIndex(x => new { x.TenantId, x.IsDefault });
        builder.HasIndex(x => x.CurrencyCode);
    }
}
