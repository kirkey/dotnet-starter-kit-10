using FSH.Modules.Ai.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pgvector.EntityFrameworkCore;

namespace FSH.Modules.Ai.Data.Configurations;

public sealed class AiChunkConfiguration : IEntityTypeConfiguration<AiChunk>
{
    public void Configure(EntityTypeBuilder<AiChunk> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.ToTable("Chunks");
        builder.HasKey(x => x.Id);

        // Id is app-assigned (Guid.CreateVersion7) and chunks attach only via the AiSource aggregate's nav
        // collection. Without ValueGeneratedNever, EF tracks the populated Guid as Modified → UPDATE-0-rows.
        builder.Property(x => x.Id).ValueGeneratedNever();

        builder.Property(x => x.Content).IsRequired();
        builder.Property(x => x.EmbeddingModel).HasMaxLength(128);
        builder.Property(x => x.Embedding).HasColumnType($"halfvec({AiChunk.EmbeddingDimensions})");

        builder.HasIndex(x => x.SourceId);
        builder.HasIndex(x => x.Ordinal);

        // Cosine HNSW index for nearest-neighbor retrieval. halfvec (not vector) because
        // HNSW caps float vectors at 2000 dimensions; half precision is the documented path
        // past that. Dimensions are fixed at AiChunk.EmbeddingDimensions; smaller provider
        // vectors are zero-padded on write (see AiChunk.AttachEmbedding).
        builder.HasIndex(x => x.Embedding)
            .HasMethod("hnsw")
            .HasOperators("halfvec_cosine_ops");

        builder.Ignore(x => x.DomainEvents);
    }
}
