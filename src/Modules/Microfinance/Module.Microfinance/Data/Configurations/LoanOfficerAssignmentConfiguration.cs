using FSH.Module.Microfinance.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Module.Microfinance.Data.Configurations;

public class LoanOfficerAssignmentConfiguration : IEntityTypeConfiguration<LoanOfficerAssignment>
{
    public void Configure(EntityTypeBuilder<LoanOfficerAssignment> builder)
    {
        builder.ToTable("LoanOfficerAssignments", "microfinance");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(256);
        builder.HasIndex(x => x.TenantId);
    }
}
