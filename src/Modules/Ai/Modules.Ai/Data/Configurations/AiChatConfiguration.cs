using FSH.Modules.Ai.Contracts.Dtos;
using FSH.Modules.Ai.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Modules.Ai.Data.Configurations;

public sealed class AiChatSessionConfiguration : IEntityTypeConfiguration<AiChatSession>
{
    public void Configure(EntityTypeBuilder<AiChatSession> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.ToTable("ChatSessions");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title).IsRequired().HasMaxLength(200);
        builder.Property(x => x.Model).IsRequired().HasMaxLength(256);
        builder.Property(x => x.Variant).HasConversion<string>().HasMaxLength(16);

        builder.HasIndex(x => x.OwnerId);
        builder.HasIndex(x => x.LastActivityUtc);

        builder.HasMany(x => x.Messages)
            .WithOne()
            .HasForeignKey(m => m.SessionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Ignore(x => x.DomainEvents);
    }
}

public sealed class AiChatMessageConfiguration : IEntityTypeConfiguration<AiChatMessage>
{
    public void Configure(EntityTypeBuilder<AiChatMessage> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.ToTable("ChatMessages");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Role).IsRequired().HasMaxLength(16);
        builder.Property(x => x.Content).IsRequired().HasColumnType("text");
        builder.Property(x => x.CitedSourcesJson).IsRequired().HasMaxLength(4096);

        builder.HasIndex(x => x.SessionId);
        builder.Ignore(x => x.DomainEvents);
    }
}
