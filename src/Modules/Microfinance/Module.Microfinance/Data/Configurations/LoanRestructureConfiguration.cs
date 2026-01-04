using FSH.Module.Microfinance.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Module.Microfinance.Data.Configurations;

public class LoanRestructureConfiguration : IEntityTypeConfiguration<LoanRestructure>
{
    public void Configure(EntityTypeBuilder<LoanRestructure> builder)
    {
        builder.ToTable("LoanRestructures", "microfinance");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(256);
        builder.HasIndex(x => x.TenantId);
    }
}
