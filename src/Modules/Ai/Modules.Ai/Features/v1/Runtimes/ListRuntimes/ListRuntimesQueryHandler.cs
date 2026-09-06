using FSH.Modules.Ai.Contracts.Dtos;
using FSH.Modules.Ai.Contracts.v1.Runtimes;
using FSH.Modules.Ai.Data;
using FSH.Modules.Ai.Features.v1.Runtimes.RefreshRuntimeCatalog;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Ai.Features.v1.Runtimes.ListRuntimes;

public sealed class ListRuntimesQueryHandler(AiDbContext db)
    : IQueryHandler<ListRuntimesQuery, IReadOnlyList<AiRuntimeDto>>
{
    public async ValueTask<IReadOnlyList<AiRuntimeDto>> Handle(ListRuntimesQuery query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        return (await db.Runtimes
            .AsNoTracking()
            .OrderBy(r => r.Family)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false))
            .Select(RefreshRuntimeCatalogCommandHandler.ToDto)
            .ToList();
    }
}
