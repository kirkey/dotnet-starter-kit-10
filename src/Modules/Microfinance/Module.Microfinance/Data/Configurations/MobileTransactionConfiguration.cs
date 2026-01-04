using FSH.Module.Microfinance.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Module.Microfinance.Data.Configurations;

public class MobileTransactionConfiguration : IEntityTypeConfiguration<MobileTransaction>
{
    public void Configure(EntityTypeBuilder<MobileTransaction> builder)
    {
        builder.ToTable("MobileTransactions", "microfinance");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(256);
        builder.HasIndex(x => x.TenantId);
    }
}
