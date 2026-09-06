using FSH.Modules.Ai.Contracts.Dtos;
using FSH.Modules.Ai.Contracts.v1.Runs;
using FSH.Modules.Ai.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Ai.Features.v1.Runs.ListAgentRuns;

public sealed class ListAgentRunsQueryHandler(AiDbContext db)
    : IQueryHandler<ListAgentRunsQuery, IReadOnlyList<AiAgentRunDto>>
{
    public async ValueTask<IReadOnlyList<AiAgentRunDto>> Handle(ListAgentRunsQuery query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        var q = db.AgentRuns.AsNoTracking().AsQueryable();
        if (query.AgentId.HasValue)
        {
            q = q.Where(r => r.AgentId == query.AgentId.Value);
        }

        if (query.Status.HasValue)
        {
            q = q.Where(r => r.Status == query.Status.Value);
        }

        return (await q
            .OrderByDescending(r => r.Id)
            .Take(100)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false))
            .Select(AiAgentRunMapper.ToDto)
            .ToList();
    }
}
