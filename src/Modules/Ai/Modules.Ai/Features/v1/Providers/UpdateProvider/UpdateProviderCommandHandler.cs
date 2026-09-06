using System.Net;
using FSH.Framework.Core.Exceptions;
using FSH.Modules.Ai.Contracts.v1.Providers;
using FSH.Modules.Ai.Data;
using FSH.Modules.Ai.Domain;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Ai.Features.v1.Providers.UpdateProvider;

public sealed class UpdateProviderCommandHandler(AiDbContext db)
    : ICommandHandler<UpdateProviderCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateProviderCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        var provider = await db.Providers
            .FirstOrDefaultAsync(p => p.Id == command.Id, cancellationToken)
            .ConfigureAwait(false)
            ?? throw new NotFoundException($"Provider {command.Id} was not found.");

        if (provider.Revision != command.Revision)
        {
            throw new CustomException(
                $"Provider '{provider.Name}' changed since revision {command.Revision} (now {provider.Revision}). Reload and retry.",
                errors: null,
                HttpStatusCode.Conflict);
        }

        var nameTaken = await db.Providers
            .AnyAsync(p => p.Id != command.Id && p.Name == command.Name.Trim(), cancellationToken)
            .ConfigureAwait(false);
        if (nameTaken)
        {
            throw new CustomException(
                $"A provider named '{command.Name.Trim()}' already exists.",
                errors: null,
                HttpStatusCode.Conflict);
        }

        provider.Update(
            command.Name,
            command.BaseUrl,
            command.ChatModel,
            command.EmbeddingModel,
            command.EmbeddingDimensions);

        // Replace the model catalog with explicit deletes + AddRange. Do NOT Clear()/AddRange
        // on the tracked navigation here: adding to the collection of an already-modified parent
        // mis-tracks the new rows as Modified (UPDATE on missing rows → concurrency failure).
        await db.ProviderModels
            .Where(m => m.ProviderId == provider.Id)
            .ExecuteDeleteAsync(cancellationToken)
            .ConfigureAwait(false);

        db.ProviderModels.AddRange(command.Models.Select(m =>
            AiProviderModel.Create(provider.Id, m.ModelId, m.DisplayName, m.SupportsChat, m.SupportsEmbeddings)));

        await db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return provider.Id;
    }
}
