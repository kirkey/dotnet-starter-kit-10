using FSH.Module.Microfinance.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Module.Microfinance.Data.Configurations;

public class CommunicationTemplateConfiguration : IEntityTypeConfiguration<CommunicationTemplate>
{
    public void Configure(EntityTypeBuilder<CommunicationTemplate> builder)
    {
        builder.ToTable("CommunicationTemplates", "microfinance");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(256);
        builder.HasIndex(x => x.TenantId);
    }
}
