using FSH.Module.Microfinance.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Module.Microfinance.Data.Configurations;

public class DocumentConfiguration : IEntityTypeConfiguration<Document>
{
    public void Configure(EntityTypeBuilder<Document> builder)
    {
        builder.ToTable("Documents", "microfinance");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(256);
        builder.HasIndex(x => x.TenantId);
    }
}
