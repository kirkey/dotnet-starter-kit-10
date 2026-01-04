using FSH.Module.Accounting.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Module.Accounting.Data.Configurations;

public class BankReconciliationConfiguration : IEntityTypeConfiguration<BankReconciliation>
{
    public void Configure(EntityTypeBuilder<BankReconciliation> builder)
    {
        builder.ToTable("BankReconciliations", "accounting");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.ReconciliationNumber).IsRequired().HasMaxLength(AccountingStringLengths.Medium);
        builder.Property(x => x.BankAccountId).IsRequired();
        builder.Property(x => x.StatementDate).IsRequired();
        builder.Property(x => x.StatementBalance).IsRequired().HasPrecision(18,2);
        builder.Property(x => x.BookBalance).IsRequired().HasPrecision(18,2);
        builder.Property(x => x.Difference).IsRequired().HasPrecision(18,2);
        builder.Property(x => x.Status).IsRequired().HasMaxLength(AccountingStringLengths.Regular);
        builder.Property(x => x.ReconciledDate);
        builder.Property(x => x.ReconciledBy);
        builder.Property(x => x.AdjustmentAmount).IsRequired().HasPrecision(18,2);
        builder.Property(x => x.AdjustmentNotes).HasMaxLength(AccountingStringLengths.XHuge);

        builder.Property(x => x.Description).HasMaxLength(AccountingStringLengths.Description);
        builder.Property(x => x.IsActive).IsRequired();
        builder.Property(x => x.TenantId).IsRequired().HasMaxLength(AccountingStringLengths.TenantId);

        builder.HasIndex(x => x.TenantId);
        builder.HasIndex(x => new { x.TenantId, x.ReconciliationNumber }).IsUnique();
        builder.HasIndex(x => new { x.TenantId, x.Status });
        builder.HasIndex(x => x.BankAccountId);
        builder.HasIndex(x => x.IsActive);
    }
}
