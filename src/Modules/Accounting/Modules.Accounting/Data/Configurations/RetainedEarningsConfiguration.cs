using FSH.Modules.Accounting.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Modules.Accounting.Data.Configurations;

public class RetainedEarningsConfiguration : IEntityTypeConfiguration<RetainedEarnings>
{
    public void Configure(EntityTypeBuilder<RetainedEarnings> builder)
    {
        builder.ToTable("RetainedEarnings", "accounting");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(AccountingStringLengths.Name);
        builder.Property(x => x.Description).HasMaxLength(AccountingStringLengths.Description);
        builder.Property(x => x.FiscalYear);
        builder.Property(x => x.OpeningBalance).HasPrecision(18, 2);
        builder.Property(x => x.ClosingBalance).HasPrecision(18, 2);
        builder.Property(x => x.IsClosed).IsRequired();
        builder.Property(x => x.ClosedOn);
        builder.Property(x => x.ClosedBy);
        builder.Property(x => x.TenantId).IsRequired().HasMaxLength(AccountingStringLengths.TenantId);
        builder.HasIndex(x => x.TenantId);
        builder.HasIndex(x => new { x.TenantId, x.Name });
        builder.HasIndex(x => x.FiscalYear);
    }
}
