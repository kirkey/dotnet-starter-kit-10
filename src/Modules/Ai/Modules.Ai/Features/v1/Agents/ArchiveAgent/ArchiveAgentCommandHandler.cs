using FSH.Framework.Core.Exceptions;
using FSH.Modules.Ai.Contracts.v1.Agents;
using FSH.Modules.Ai.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Ai.Features.v1.Agents.ArchiveAgent;

public sealed class ArchiveAgentCommandHandler(AiDbContext db)
    : ICommandHandler<ArchiveAgentCommand, Guid>
{
    public async ValueTask<Guid> Handle(ArchiveAgentCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        var agent = await db.Agents
            .FirstOrDefaultAsync(a => a.Id == command.Id, cancellationToken)
            .ConfigureAwait(false)
            ?? throw new NotFoundException($"Agent {command.Id} was not found.");

        agent.Archive();
        await db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return agent.Id;
    }
}
