using FSH.Framework.Shared.Multitenancy;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Module.Multitenancy.Data.Configurations;

public class AppTenantInfoConfiguration : IEntityTypeConfiguration<AppTenantInfo>
{
    public void Configure(EntityTypeBuilder<AppTenantInfo> builder)
    {
        builder.ToTable("Tenants", MultitenancyConstants.Schema);
    }
}
