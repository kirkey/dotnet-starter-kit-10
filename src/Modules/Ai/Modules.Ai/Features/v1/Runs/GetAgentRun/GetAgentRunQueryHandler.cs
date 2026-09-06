using FSH.Framework.Core.Exceptions;
using FSH.Modules.Ai.Contracts.Dtos;
using FSH.Modules.Ai.Contracts.v1.Runs;
using FSH.Modules.Ai.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Ai.Features.v1.Runs.GetAgentRun;

public sealed class GetAgentRunQueryHandler(AiDbContext db)
    : IQueryHandler<GetAgentRunQuery, AiAgentRunDto>
{
    public async ValueTask<AiAgentRunDto> Handle(GetAgentRunQuery query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        var run = await db.AgentRuns
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == query.Id, cancellationToken)
            .ConfigureAwait(false)
            ?? throw new NotFoundException($"Run {query.Id} was not found.");

        return AiAgentRunMapper.ToDto(run);
    }
}
