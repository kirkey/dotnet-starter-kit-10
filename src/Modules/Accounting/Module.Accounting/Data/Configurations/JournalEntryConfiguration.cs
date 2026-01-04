using FSH.Module.Accounting.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Module.Accounting.Data.Configurations;

public class JournalEntryConfiguration : IEntityTypeConfiguration<JournalEntry>
{
    public void Configure(EntityTypeBuilder<JournalEntry> builder)
    {
        builder.ToTable("JournalEntries", "accounting");
        builder.HasKey(x => x.Id);
        
        // Core Properties
        builder.Property(x => x.EntryNumber).IsRequired().HasMaxLength(AccountingStringLengths.JournalEntryNumber);
        builder.Property(x => x.EntryDate).IsRequired();
        builder.Property(x => x.EntryType).IsRequired().HasMaxLength(AccountingStringLengths.Medium);
        builder.Property(x => x.ReferenceNumber).IsRequired().HasMaxLength(AccountingStringLengths.Medium);
        builder.Property(x => x.ReferenceType).HasMaxLength(AccountingStringLengths.Medium);
        
        // Financial Properties
        builder.Property(x => x.TotalDebit).IsRequired().HasPrecision(18, 2);
        builder.Property(x => x.TotalCredit).IsRequired().HasPrecision(18, 2);
        
        // Period & Status
        builder.Property(x => x.FiscalPeriodId).IsRequired();
        builder.Property(x => x.Status).IsRequired().HasMaxLength(AccountingStringLengths.Regular);
        builder.Property(x => x.PostedDate);
        builder.Property(x => x.PostedBy);
        builder.Property(x => x.ApprovedDate);
        builder.Property(x => x.ApprovedBy);
        
        // Reversal
        builder.Property(x => x.IsReversed).IsRequired();
        builder.Property(x => x.ReversedEntryId);
        builder.Property(x => x.ReversedDate);
        
        // Documentation
        builder.Property(x => x.Description).HasMaxLength(AccountingStringLengths.Description);
        builder.Property(x => x.Notes).HasMaxLength(AccountingStringLengths.XHuge);
        builder.Property(x => x.Memo).HasMaxLength(AccountingStringLengths.Large);
        
        // Audit & Status
        builder.Property(x => x.IsActive).IsRequired();
        builder.Property(x => x.TenantId).IsRequired().HasMaxLength(AccountingStringLengths.TenantId);
        
        // Indexes
        builder.HasIndex(x => x.TenantId);
        builder.HasIndex(x => new { x.TenantId, x.EntryNumber }).IsUnique();
        builder.HasIndex(x => new { x.TenantId, x.EntryDate });
        builder.HasIndex(x => new { x.TenantId, x.Status });
        builder.HasIndex(x => x.FiscalPeriodId);
        builder.HasIndex(x => x.IsActive);
        builder.HasIndex(x => x.ReferenceNumber);
        
        // Ignore navigation properties (will be configured separately)
        builder.Ignore(x => x.Lines);
    }
}
