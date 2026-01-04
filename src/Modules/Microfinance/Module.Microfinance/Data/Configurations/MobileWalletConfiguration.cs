using FSH.Module.Microfinance.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Module.Microfinance.Data.Configurations;

public class MobileWalletConfiguration : IEntityTypeConfiguration<MobileWallet>
{
    public void Configure(EntityTypeBuilder<MobileWallet> builder)
    {
        builder.ToTable("MobileWallets", "microfinance");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(256);
        builder.HasIndex(x => x.TenantId);
    }
}
