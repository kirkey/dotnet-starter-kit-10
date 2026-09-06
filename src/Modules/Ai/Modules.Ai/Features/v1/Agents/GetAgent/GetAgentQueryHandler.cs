using FSH.Framework.Core.Exceptions;
using FSH.Modules.Ai.Contracts.Dtos;
using FSH.Modules.Ai.Contracts.v1.Agents;
using FSH.Modules.Ai.Data;
using FSH.Modules.Ai.Features.v1.Agents;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Ai.Features.v1.Agents.GetAgent;

public sealed class GetAgentQueryHandler(AiDbContext db)
    : IQueryHandler<GetAgentQuery, AiAgentDto>
{
    public async ValueTask<AiAgentDto> Handle(GetAgentQuery query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        var agent = await db.Agents
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == query.Id, cancellationToken)
            .ConfigureAwait(false)
            ?? throw new NotFoundException($"Agent {query.Id} was not found.");

        return AiAgentMapper.ToDto(agent);
    }
}
