using FSH.Module.Microfinance.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Module.Microfinance.Data.Configurations;

public class BranchTargetConfiguration : IEntityTypeConfiguration<BranchTarget>
{
    public void Configure(EntityTypeBuilder<BranchTarget> builder)
    {
        builder.ToTable("BranchTargets", "microfinance");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(256);
        builder.HasIndex(x => x.TenantId);
    }
}
