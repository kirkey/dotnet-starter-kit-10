using FSH.Modules.Ai.Contracts.Dtos;
using FSH.Modules.Ai.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Modules.Ai.Data.Configurations;

public sealed class AiProviderConfiguration : IEntityTypeConfiguration<AiProvider>
{
    public void Configure(EntityTypeBuilder<AiProvider> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.ToTable("Providers");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
        builder.Property(x => x.ProviderType).HasConversion<string>().HasMaxLength(32);
        builder.Property(x => x.BaseUrl).HasMaxLength(2048);
        builder.Property(x => x.ChatModel).HasMaxLength(256);
        builder.Property(x => x.EmbeddingModel).HasMaxLength(256);
        builder.Property(x => x.Revision).IsConcurrencyToken();

        builder.HasIndex(x => x.IsDefaultChat);
        builder.HasIndex(x => x.IsDefaultEmbedding);

        builder.HasMany(x => x.Models)
            .WithOne()
            .HasForeignKey(m => m.ProviderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Ignore(x => x.DomainEvents);
    }
}

public sealed class AiProviderModelConfiguration : IEntityTypeConfiguration<AiProviderModel>
{
    public void Configure(EntityTypeBuilder<AiProviderModel> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.ToTable("ProviderModels");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.ModelId).IsRequired().HasMaxLength(256);
        builder.Property(x => x.DisplayName).IsRequired().HasMaxLength(256);

        builder.HasIndex(x => x.ProviderId);
        builder.Ignore(x => x.DomainEvents);
    }
}

public sealed class AiProviderSecretConfiguration : IEntityTypeConfiguration<AiProviderSecret>
{
    public void Configure(EntityTypeBuilder<AiProviderSecret> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.ToTable("ProviderSecrets");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.KeyName).IsRequired().HasMaxLength(128);
        builder.Property(x => x.ProtectedValue).IsRequired();

        builder.HasIndex(x => x.ProviderId);
        builder.Ignore(x => x.DomainEvents);
    }
}
