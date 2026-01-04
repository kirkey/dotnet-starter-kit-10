using FSH.Module.Microfinance.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Module.Microfinance.Data.Configurations;

public class StaffTrainingConfiguration : IEntityTypeConfiguration<StaffTraining>
{
    public void Configure(EntityTypeBuilder<StaffTraining> builder)
    {
        builder.ToTable("StaffTrainings", "microfinance");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(256);
        builder.HasIndex(x => x.TenantId);
    }
}
