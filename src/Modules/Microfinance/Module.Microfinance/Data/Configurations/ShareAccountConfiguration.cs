using FSH.Module.Microfinance.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Module.Microfinance.Data.Configurations;

public class ShareAccountConfiguration : IEntityTypeConfiguration<ShareAccount>
{
    public void Configure(EntityTypeBuilder<ShareAccount> builder)
    {
        builder.ToTable("ShareAccounts", "microfinance");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(256);
        builder.HasIndex(x => x.TenantId);
    }
}
