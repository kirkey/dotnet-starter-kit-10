using FSH.Modules.Ai.Contracts.Dtos;
using FSH.Modules.Ai.Contracts.v1.Agents;
using FSH.Modules.Ai.Data;
using FSH.Modules.Ai.Features.v1.Agents;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Ai.Features.v1.Agents.ListAgents;

public sealed class ListAgentsQueryHandler(AiDbContext db)
    : IQueryHandler<ListAgentsQuery, IReadOnlyList<AiAgentDto>>
{
    public async ValueTask<IReadOnlyList<AiAgentDto>> Handle(ListAgentsQuery query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        var q = db.Agents.AsNoTracking().AsQueryable();
        if (query.DepartmentId.HasValue)
        {
            q = q.Where(a => a.DepartmentId == query.DepartmentId.Value);
        }

        if (!query.IncludeArchived)
        {
            q = q.Where(a => !a.IsArchived);
        }

        return (await q
            .OrderBy(a => a.Name)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false))
            .Select(AiAgentMapper.ToDto)
            .ToList();
    }
}
