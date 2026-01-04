using FSH.Modules.Accounting.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Modules.Accounting.Data.Configurations;

public class InterCompanyTransactionConfiguration : IEntityTypeConfiguration<InterCompanyTransaction>
{
    public void Configure(EntityTypeBuilder<InterCompanyTransaction> builder)
    {
        builder.ToTable("InterCompanyTransactions", "accounting");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(AccountingStringLengths.Name);
        builder.Property(x => x.Description).HasMaxLength(AccountingStringLengths.Description);
        builder.Property(x => x.TenantId).IsRequired().HasMaxLength(AccountingStringLengths.TenantId);
        builder.HasIndex(x => x.TenantId);
        builder.HasIndex(x => new { x.TenantId, x.Name });
    }
}
