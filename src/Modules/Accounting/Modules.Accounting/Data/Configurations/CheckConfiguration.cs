using FSH.Modules.Accounting.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Modules.Accounting.Data.Configurations;

public class CheckConfiguration : IEntityTypeConfiguration<Check>
{
    public void Configure(EntityTypeBuilder<Check> builder)
    {
        builder.ToTable("Checks", "accounting");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.CheckNumber).IsRequired().HasMaxLength(AccountingStringLengths.CheckNumber);
        builder.Property(x => x.CheckDate).IsRequired();
        builder.Property(x => x.CheckType).IsRequired().HasMaxLength(AccountingStringLengths.Medium);

        builder.Property(x => x.PayeeId);
        builder.Property(x => x.PayeeName).IsRequired().HasMaxLength(AccountingStringLengths.Name);

        builder.Property(x => x.BankAccountId).IsRequired();
        builder.Property(x => x.AccountNumber).IsRequired().HasMaxLength(AccountingStringLengths.Medium);

        builder.Property(x => x.Amount).IsRequired().HasPrecision(18, 2);

        builder.Property(x => x.Status).IsRequired().HasMaxLength(AccountingStringLengths.Regular);
        builder.Property(x => x.PrintedDate);
        builder.Property(x => x.ClearedDate);
        builder.Property(x => x.ClearedBy);

        builder.Property(x => x.JournalEntryId);

        builder.Property(x => x.ReferenceNumber).HasMaxLength(AccountingStringLengths.Medium);
        builder.Property(x => x.Notes).HasMaxLength(AccountingStringLengths.XHuge);

        builder.Property(x => x.IsActive).IsRequired();
        builder.Property(x => x.TenantId).IsRequired().HasMaxLength(AccountingStringLengths.TenantId);

        builder.HasIndex(x => x.TenantId);
        builder.HasIndex(x => new { x.TenantId, x.CheckNumber }).IsUnique();
        builder.HasIndex(x => new { x.TenantId, x.Status });
        builder.HasIndex(x => x.BankAccountId);
        builder.HasIndex(x => x.IsActive);

    }
}
