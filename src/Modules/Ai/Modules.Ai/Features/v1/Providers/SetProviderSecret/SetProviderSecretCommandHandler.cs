using FSH.Framework.Core.Exceptions;
using FSH.Modules.Ai.Contracts.v1.Providers;
using FSH.Modules.Ai.Data;
using FSH.Modules.Ai.Domain;
using FSH.Modules.Ai.Services;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Ai.Features.v1.Providers.SetProviderSecret;

public sealed class SetProviderSecretCommandHandler(
    AiDbContext db,
    IAiSecretProtector secrets)
    : ICommandHandler<SetProviderSecretCommand, Guid>
{
    public async ValueTask<Guid> Handle(SetProviderSecretCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        var exists = await db.Providers
            .AnyAsync(p => p.Id == command.ProviderId, cancellationToken)
            .ConfigureAwait(false);
        if (!exists)
        {
            throw new NotFoundException($"Provider {command.ProviderId} was not found.");
        }

        var keyName = string.IsNullOrWhiteSpace(command.KeyName) ? "apiKey" : command.KeyName.Trim();
        var protectedValue = secrets.Protect(command.Value)
            ?? throw new InvalidOperationException("Secret protection failed.");

        var secret = await db.ProviderSecrets
            .FirstOrDefaultAsync(
                s => s.ProviderId == command.ProviderId && s.KeyName == keyName, cancellationToken)
            .ConfigureAwait(false);
        if (secret is null)
        {
            db.ProviderSecrets.Add(AiProviderSecret.Create(command.ProviderId, keyName, protectedValue));
        }
        else
        {
            secret.Replace(protectedValue);
        }

        await db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return command.ProviderId;
    }
}
