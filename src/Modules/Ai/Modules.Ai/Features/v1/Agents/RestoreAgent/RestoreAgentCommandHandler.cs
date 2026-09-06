using FSH.Framework.Core.Exceptions;
using FSH.Modules.Ai.Contracts.v1.Agents;
using FSH.Modules.Ai.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Ai.Features.v1.Agents.RestoreAgent;

public sealed class RestoreAgentCommandHandler(AiDbContext db)
    : ICommandHandler<RestoreAgentCommand, Guid>
{
    public async ValueTask<Guid> Handle(RestoreAgentCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        var agent = await db.Agents
            .FirstOrDefaultAsync(a => a.Id == command.Id, cancellationToken)
            .ConfigureAwait(false)
            ?? throw new NotFoundException($"Agent {command.Id} was not found.");

        agent.Restore();
        await db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return agent.Id;
    }
}
