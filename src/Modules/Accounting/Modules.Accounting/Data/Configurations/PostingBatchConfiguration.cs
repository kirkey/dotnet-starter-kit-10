using FSH.Modules.Accounting.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Modules.Accounting.Data.Configurations;

public class PostingBatchConfiguration : IEntityTypeConfiguration<PostingBatch>
{
    public void Configure(EntityTypeBuilder<PostingBatch> builder)
    {
        builder.ToTable("PostingBatches", "accounting");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(AccountingStringLengths.Name);
        builder.Property(x => x.BatchDate).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(AccountingStringLengths.Description);
        builder.Property(x => x.Status).IsRequired().HasMaxLength(AccountingStringLengths.Regular);
        builder.Property(x => x.TotalDebits).HasPrecision(18, 2);
        builder.Property(x => x.TotalCredits).HasPrecision(18, 2);
        builder.Property(x => x.EntryCount);
        builder.Property(x => x.PostedOn);
        builder.Property(x => x.PostedBy);
        builder.Property(x => x.ReversedOn);
        builder.Property(x => x.ReversedBy);
        builder.Property(x => x.IsActive).IsRequired();
        builder.Property(x => x.TenantId).IsRequired().HasMaxLength(AccountingStringLengths.TenantId);
        builder.HasIndex(x => x.TenantId);
        builder.HasIndex(x => new { x.TenantId, x.Name });
        builder.HasIndex(x => x.Status);
        builder.HasIndex(x => x.PostedOn);
        builder.HasIndex(x => x.ReversedOn);
    }
}
