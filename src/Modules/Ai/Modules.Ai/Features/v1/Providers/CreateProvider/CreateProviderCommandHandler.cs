using System.Net;
using FSH.Framework.Core.Exceptions;
using FSH.Modules.Ai.Contracts.Dtos;
using FSH.Modules.Ai.Contracts.v1.Providers;
using FSH.Modules.Ai.Data;
using FSH.Modules.Ai.Domain;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Ai.Features.v1.Providers.CreateProvider;

public sealed class CreateProviderCommandHandler(AiDbContext db)
    : ICommandHandler<CreateProviderCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateProviderCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        var nameTaken = await db.Providers
            .AnyAsync(p => p.Name == command.Name.Trim(), cancellationToken)
            .ConfigureAwait(false);
        if (nameTaken)
        {
            throw new CustomException(
                $"A provider named '{command.Name.Trim()}' already exists.",
                errors: null,
                HttpStatusCode.Conflict);
        }

        var provider = AiProvider.Create(
            command.Name,
            command.ProviderType,
            command.BaseUrl,
            command.ChatModel,
            command.EmbeddingModel,
            command.EmbeddingDimensions);
        provider.SetModels(command.Models.Select(m =>
            AiProviderModel.Create(provider.Id, m.ModelId, m.DisplayName, m.SupportsChat, m.SupportsEmbeddings)));

        db.Providers.Add(provider);
        await db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return provider.Id;
    }
}
