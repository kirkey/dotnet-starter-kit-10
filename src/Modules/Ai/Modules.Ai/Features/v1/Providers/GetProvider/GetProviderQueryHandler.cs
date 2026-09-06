using FSH.Framework.Core.Exceptions;
using FSH.Modules.Ai.Contracts.Dtos;
using FSH.Modules.Ai.Contracts.v1.Providers;
using FSH.Modules.Ai.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Ai.Features.v1.Providers.GetProvider;

public sealed class GetProviderQueryHandler(AiDbContext db)
    : IQueryHandler<GetProviderQuery, AiProviderDto>
{
    public async ValueTask<AiProviderDto> Handle(GetProviderQuery query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        var provider = await db.Providers
            .Include(p => p.Models)
            .FirstOrDefaultAsync(p => p.Id == query.Id, cancellationToken)
            .ConfigureAwait(false)
            ?? throw new NotFoundException($"Provider {query.Id} was not found.");

        var hasApiKey = await db.ProviderSecrets
            .AnyAsync(s => s.ProviderId == provider.Id, cancellationToken)
            .ConfigureAwait(false);

        return AiProviderMapper.ToDto(provider, hasApiKey);
    }
}
