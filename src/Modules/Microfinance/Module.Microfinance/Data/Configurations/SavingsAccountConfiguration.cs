using FSH.Module.Microfinance.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Module.Microfinance.Data.Configurations;

public class SavingsAccountConfiguration : IEntityTypeConfiguration<SavingsAccount>
{
    public void Configure(EntityTypeBuilder<SavingsAccount> builder)
    {
        builder.ToTable("SavingsAccounts", "microfinance");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(256);
        builder.HasIndex(x => x.TenantId);
    }
}
