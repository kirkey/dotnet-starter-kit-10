using FSH.Module.Accounting.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Module.Accounting.Data.Configurations;

public class FiscalPeriodCloseConfiguration : IEntityTypeConfiguration<FiscalPeriodClose>
{
    public void Configure(EntityTypeBuilder<FiscalPeriodClose> builder)
    {
        builder.ToTable("FiscalPeriodClose", "accounting");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.FiscalPeriodId).IsRequired();
        builder.Property(x => x.FiscalYear).IsRequired();
        builder.Property(x => x.PeriodName).IsRequired().HasMaxLength(AccountingStringLengths.Medium);
        builder.Property(x => x.StartDate).IsRequired();
        builder.Property(x => x.EndDate).IsRequired();

        builder.Property(x => x.CloseDate);
        builder.Property(x => x.Status).IsRequired().HasMaxLength(AccountingStringLengths.Small);
        builder.Property(x => x.RetainedEarnings).IsRequired().HasPrecision(18,2);
        builder.Property(x => x.ClosingJournalEntryId);
        builder.Property(x => x.ClosedBy);

        builder.Property(x => x.AdjustmentAmount).HasPrecision(18,2);
        builder.Property(x => x.AdjustmentNotes).HasMaxLength(AccountingStringLengths.XHuge);

        builder.Property(x => x.Description).HasMaxLength(AccountingStringLengths.Description);
        builder.Property(x => x.IsActive).IsRequired();
        builder.Property(x => x.TenantId).IsRequired().HasMaxLength(AccountingStringLengths.TenantId);

        builder.HasIndex(x => x.TenantId);
        builder.HasIndex(x => new { x.TenantId, x.FiscalYear, x.PeriodName }).IsUnique();
        builder.HasIndex(x => new { x.TenantId, x.Status });
        builder.HasIndex(x => x.FiscalPeriodId);
        builder.HasIndex(x => x.IsActive);
    }
}
