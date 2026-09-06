using System.Net;
using FSH.Framework.Core.Exceptions;
using FSH.Modules.Ai.Contracts.v1.Providers;
using FSH.Modules.Ai.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Ai.Features.v1.Providers.DeleteProvider;

public sealed class DeleteProviderCommandHandler(AiDbContext db)
    : ICommandHandler<DeleteProviderCommand, Guid>
{
    public async ValueTask<Guid> Handle(DeleteProviderCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        var provider = await db.Providers
            .Include(p => p.Models)
            .FirstOrDefaultAsync(p => p.Id == command.Id, cancellationToken)
            .ConfigureAwait(false)
            ?? throw new NotFoundException($"Provider {command.Id} was not found.");

        var servedModels = provider.Models.Select(m => m.ModelId)
            .Append(provider.ChatModel)
            .Append(provider.EmbeddingModel)
            .Where(m => !string.IsNullOrWhiteSpace(m))
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var boundAgent = await db.Agents
            .Where(a => !a.IsArchived)
            .Select(a => new { a.Name, a.Model })
            .FirstOrDefaultAsync(a => servedModels.Contains(a.Model), cancellationToken)
            .ConfigureAwait(false);
        if (boundAgent is not null)
        {
            throw new CustomException(
                $"Provider '{provider.Name}' cannot be deleted: agent '{boundAgent.Name}' uses model '{boundAgent.Model}'.",
                errors: null,
                HttpStatusCode.Conflict);
        }

        await db.ProviderSecrets
            .Where(s => s.ProviderId == provider.Id)
            .ExecuteDeleteAsync(cancellationToken)
            .ConfigureAwait(false);

        db.Providers.Remove(provider);
        await db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return provider.Id;
    }
}
