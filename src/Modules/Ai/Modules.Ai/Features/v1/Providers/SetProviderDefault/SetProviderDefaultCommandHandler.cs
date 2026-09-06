using FSH.Framework.Core.Exceptions;
using FSH.Modules.Ai.Contracts.v1.Providers;
using FSH.Modules.Ai.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Ai.Features.v1.Providers.SetProviderDefault;

public sealed class SetProviderDefaultCommandHandler(AiDbContext db)
    : ICommandHandler<SetProviderDefaultCommand, Guid>
{
    public async ValueTask<Guid> Handle(SetProviderDefaultCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        var provider = await db.Providers
            .FirstOrDefaultAsync(p => p.Id == command.ProviderId, cancellationToken)
            .ConfigureAwait(false)
            ?? throw new NotFoundException($"Provider {command.ProviderId} was not found.");

        if (command.Chat)
        {
            var others = await db.Providers
                .Where(p => p.Id != provider.Id && p.IsDefaultChat)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
            foreach (var other in others)
            {
                other.ClearDefault(chat: true, embedding: false);
            }
        }

        if (command.Embedding)
        {
            var others = await db.Providers
                .Where(p => p.Id != provider.Id && p.IsDefaultEmbedding)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
            foreach (var other in others)
            {
                other.ClearDefault(chat: false, embedding: true);
            }
        }

        provider.SetAsDefault(command.Chat, command.Embedding);
        await db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return provider.Id;
    }
}
