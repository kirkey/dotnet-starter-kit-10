using System.Net;
using FSH.Framework.Core.Exceptions;
using FSH.Modules.Ai.Contracts.Dtos;
using FSH.Modules.Ai.Contracts.v1.Runs;
using FSH.Modules.Ai.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Ai.Features.v1.Runs.EndAgentRun;

public sealed class EndAgentRunCommandHandler(AiDbContext db)
    : ICommandHandler<EndAgentRunCommand, Guid>
{
    public async ValueTask<Guid> Handle(EndAgentRunCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        var run = await db.AgentRuns
            .FirstOrDefaultAsync(r => r.Id == command.Id, cancellationToken)
            .ConfigureAwait(false)
            ?? throw new NotFoundException($"Run {command.Id} was not found.");

        if (run.Status is AgentRunStatus.Completed or AgentRunStatus.Failed or AgentRunStatus.Cancelled)
        {
            throw new CustomException(
                $"Run is already {run.Status}; only queued or running runs can be cancelled.",
                errors: null,
                HttpStatusCode.Conflict);
        }

        run.MarkCancelled();
        await db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return run.Id;
    }
}
