using FSH.Framework.Shared.Multitenancy;
using FSH.Module.Multitenancy.Provisioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Module.Multitenancy.Data.Configurations;

public class TenantProvisioningStepConfiguration : IEntityTypeConfiguration<TenantProvisioningStep>
{
    public void Configure(EntityTypeBuilder<TenantProvisioningStep> builder)
    {
        builder.ToTable("TenantProvisioningSteps", MultitenancyConstants.Schema);
    }
}
