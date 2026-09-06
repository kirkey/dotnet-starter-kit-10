using FSH.Framework.Persistence;
using FSH.Modules.Ai.Contracts.Dtos;
using FSH.Modules.Ai.Data;
using FSH.Modules.Ai.Domain;
using FSH.Modules.Ai.Services;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Ai.Data;

internal sealed class AiDbInitializer(AiDbContext context) : IDbInitializer
{
    public async Task MigrateAsync(CancellationToken cancellationToken)
    {
        if ((await context.Database.GetPendingMigrationsAsync(cancellationToken).ConfigureAwait(false)).Any())
        {
            await context.Database.MigrateAsync(cancellationToken).ConfigureAwait(false);
        }
    }

    public async Task SeedAsync(CancellationToken cancellationToken)
    {
        // Zero-config default: the built-in deterministic local provider serves chat and
        // embeddings until the operator registers a real provider. Runs per tenant (the
        // migrator invokes initializers in tenant scope), guarded by existence.
        if (await context.Providers.AnyAsync(cancellationToken).ConfigureAwait(false))
        {
            return;
        }

        var local = AiProvider.Create(
            "Local (built-in)",
            AiProviderType.Local,
            baseUrl: null,
            chatModel: LocalChatClient.LocalModelName,
            embeddingModel: LocalEmbeddingClient.LocalModelName,
            embeddingDimensions: AiChunk.EmbeddingDimensions);
        local.SetModels([
            AiProviderModel.Create(local.Id, LocalChatClient.LocalModelName, "Local chat", true, false),
            AiProviderModel.Create(local.Id, LocalEmbeddingClient.LocalModelName, "Local embeddings", false, true),
        ]);
        local.SetAsDefault(chat: true, embedding: true);
        context.Providers.Add(local);
        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }
}
