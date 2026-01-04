using FSH.Module.Microfinance.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Module.Microfinance.Data.Configurations;

public class MemberGroupConfiguration : IEntityTypeConfiguration<MemberGroup>
{
    public void Configure(EntityTypeBuilder<MemberGroup> builder)
    {
        builder.ToTable("MemberGroups", "microfinance");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(256);
        builder.HasIndex(x => x.TenantId);
    }
}
