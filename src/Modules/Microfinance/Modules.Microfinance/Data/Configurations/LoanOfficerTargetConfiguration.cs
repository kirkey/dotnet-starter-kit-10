using FSH.Modules.Microfinance.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Modules.Microfinance.Data.Configurations;

public class LoanOfficerTargetConfiguration : IEntityTypeConfiguration<LoanOfficerTarget>
{
    public void Configure(EntityTypeBuilder<LoanOfficerTarget> builder)
    {
        builder.ToTable("LoanOfficerTargets", "microfinance");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(256);
        builder.HasIndex(x => x.TenantId);
    }
}
