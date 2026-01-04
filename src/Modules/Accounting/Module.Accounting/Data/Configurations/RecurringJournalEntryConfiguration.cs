using FSH.Module.Accounting.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Module.Accounting.Data.Configurations;

public class RecurringJournalEntryConfiguration : IEntityTypeConfiguration<RecurringJournalEntry>
{
    public void Configure(EntityTypeBuilder<RecurringJournalEntry> builder)
    {
        builder.ToTable("RecurringJournalEntries", "accounting");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(AccountingStringLengths.Name);
        builder.Property(x => x.Frequency).IsRequired().HasMaxLength(AccountingStringLengths.Regular);
        builder.Property(x => x.NextRunDate);
        builder.Property(x => x.LastRunDate);
        builder.Property(x => x.FiscalPeriodId);
        builder.Property(x => x.IsAutoPost).IsRequired();
        builder.Property(x => x.ApprovedBy);
        builder.Property(x => x.ApprovedOn);
        builder.Property(x => x.Description).HasMaxLength(AccountingStringLengths.Description);
        builder.Property(x => x.TenantId).IsRequired().HasMaxLength(AccountingStringLengths.TenantId);
        builder.HasIndex(x => x.TenantId);
        builder.HasIndex(x => new { x.TenantId, x.Name });
        builder.HasIndex(x => x.NextRunDate);
        builder.HasIndex(x => x.Frequency);
    }
}
