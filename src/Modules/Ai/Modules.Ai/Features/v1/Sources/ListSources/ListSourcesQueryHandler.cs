using FSH.Framework.Shared.Persistence;
using FSH.Modules.Ai.Contracts.Dtos;
using FSH.Modules.Ai.Contracts.v1.Sources;
using FSH.Modules.Ai.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Ai.Features.v1.Sources.ListSources;

public sealed class ListSourcesQueryHandler(AiDbContext db)
    : IQueryHandler<ListSourcesQuery, PagedResponse<AiSourceDto>>
{
    public async ValueTask<PagedResponse<AiSourceDto>> Handle(ListSourcesQuery query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        int page = query.PageNumber is < 1 or null ? 1 : query.PageNumber.Value;
        int size = query.PageSize is < 1 or > 200 or null ? 20 : query.PageSize.Value;

        var q = db.Sources.AsNoTracking().AsQueryable();
        if (query.Kind.HasValue)
        {
            q = q.Where(s => s.Kind == query.Kind.Value);
        }

        if (query.Status.HasValue)
        {
            q = q.Where(s => s.Status == query.Status.Value);
        }

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            string term = $"%{query.Search.Trim()}%";
            q = q.Where(s => EF.Functions.ILike(s.Name, term) || EF.Functions.ILike(s.SourceRef, term));
        }

        q = (query.Sort?.ToUpperInvariant()) switch
        {
            "NAME_DESC" or "NAME-" or "-NAME" => q.OrderByDescending(s => s.Name),
            "STATUS" => q.OrderBy(s => s.Status).ThenBy(s => s.Name),
            "STATUS_DESC" or "STATUS-" or "-STATUS" => q.OrderByDescending(s => s.Status).ThenBy(s => s.Name),
            _ => q.OrderBy(s => s.Name),
        };

        long total = await q.LongCountAsync(cancellationToken).ConfigureAwait(false);
        var items = await q
            .Skip((page - 1) * size)
            .Take(size)
            .Select(s => new AiSourceDto(s.Id, s.Name, s.Kind, s.Status, s.SourceRef, s.MdStorageKey != null, s.ErrorReason))
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        return new PagedResponse<AiSourceDto>
        {
            Items = items,
            PageNumber = page,
            PageSize = size,
            TotalCount = total,
            TotalPages = (int)Math.Ceiling(total / (double)size)
        };
    }
}
