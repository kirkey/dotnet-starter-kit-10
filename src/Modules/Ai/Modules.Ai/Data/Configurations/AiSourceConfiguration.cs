using FSH.Modules.Ai.Contracts.Dtos;
using FSH.Modules.Ai.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Modules.Ai.Data.Configurations;

public sealed class AiSourceConfiguration : IEntityTypeConfiguration<AiSource>
{
    public void Configure(EntityTypeBuilder<AiSource> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.ToTable("Sources");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
        builder.Property(x => x.Kind).HasConversion<string>().HasMaxLength(16);
        builder.Property(x => x.Status).HasConversion<string>().HasMaxLength(16);
        builder.Property(x => x.SourceRef).IsRequired().HasMaxLength(2048);
        builder.Property(x => x.MdStorageKey).HasMaxLength(1024);
        builder.Property(x => x.ErrorReason).HasMaxLength(2048);

        builder.HasIndex(x => x.Status);
        builder.HasIndex(x => x.IsDeleted);
        // One row per ingested file/URL: file sources key on the FileAsset id, web sources on the
        // normalized URL. Guards against double-ingestion on event redelivery (Inbox dedups most,
        // this covers the rest).
        builder.HasIndex(x => x.SourceRef).IsUnique();
        builder.Property(x => x.DeletedBy).HasMaxLength(64);

        builder.HasMany(x => x.Chunks)
            .WithOne()
            .HasForeignKey(c => c.SourceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Ignore(x => x.DomainEvents);
    }
}
