using FSH.Modules.Accounting.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Modules.Accounting.Data.Configurations;

public class JournalEntryLineConfiguration : IEntityTypeConfiguration<JournalEntryLine>
{
    public void Configure(EntityTypeBuilder<JournalEntryLine> builder)
    {
        builder.ToTable("JournalEntryLines", "accounting");
        builder.HasKey(x => x.Id);
        
        // Parent Reference
        builder.Property(x => x.JournalEntryId).IsRequired();
        
        // Line Identification
        builder.Property(x => x.LineNumber).IsRequired();
        
        // Account Reference
        builder.Property(x => x.AccountId).IsRequired();
        builder.Property(x => x.AccountCode).IsRequired().HasMaxLength(AccountingStringLengths.AccountCode);
        builder.Property(x => x.AccountName).IsRequired().HasMaxLength(AccountingStringLengths.AccountName);
        
        // Financial Properties
        builder.Property(x => x.Debit).IsRequired().HasPrecision(18, 2);
        builder.Property(x => x.Credit).IsRequired().HasPrecision(18, 2);
        builder.Property(x => x.Amount).IsRequired().HasPrecision(18, 2);
        builder.Property(x => x.TransactionType).IsRequired().HasMaxLength(AccountingStringLengths.Regular);
        
        // Reference & Documentation
        builder.Property(x => x.ReferenceNumber).HasMaxLength(AccountingStringLengths.Medium);
        builder.Property(x => x.Description).HasMaxLength(AccountingStringLengths.Description);
        builder.Property(x => x.Notes).HasMaxLength(AccountingStringLengths.XHuge);
        
        // Cost Allocation
        builder.Property(x => x.CostCenterId);
        builder.Property(x => x.DepartmentId);
        builder.Property(x => x.ProjectId);
        
        // Audit & Status
        builder.Property(x => x.IsActive).IsRequired();
        builder.Property(x => x.TenantId).IsRequired().HasMaxLength(AccountingStringLengths.TenantId);
        
        // Indexes
        builder.HasIndex(x => x.TenantId);
        builder.HasIndex(x => new { x.TenantId, x.JournalEntryId });
        builder.HasIndex(x => new { x.JournalEntryId, x.LineNumber }).IsUnique();
        builder.HasIndex(x => x.AccountId);
        builder.HasIndex(x => x.IsActive);
        
        // Ignore navigation properties
        builder.Ignore(x => x.JournalEntry);
        builder.Ignore(x => x.Account);
    }
}
