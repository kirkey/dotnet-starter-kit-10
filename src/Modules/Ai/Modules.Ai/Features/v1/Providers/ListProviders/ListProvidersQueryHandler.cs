using FSH.Modules.Ai.Contracts.Dtos;
using FSH.Modules.Ai.Contracts.v1.Providers;
using FSH.Modules.Ai.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Ai.Features.v1.Providers.ListProviders;

public sealed class ListProvidersQueryHandler(AiDbContext db)
    : IQueryHandler<ListProvidersQuery, IReadOnlyList<AiProviderDto>>
{
    public async ValueTask<IReadOnlyList<AiProviderDto>> Handle(ListProvidersQuery query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        var providers = await db.Providers
            .Include(p => p.Models)
            .OrderBy(p => p.Name)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        var keyedProviderIds = await db.ProviderSecrets
            .Select(s => s.ProviderId)
            .Distinct()
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        return providers
            .Select(p => AiProviderMapper.ToDto(p, keyedProviderIds.Contains(p.Id)))
            .ToList();
    }
}
