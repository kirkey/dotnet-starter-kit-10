using System.Net;
using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Framework.Jobs.Services;
using FSH.Modules.Ai.Contracts.Dtos;
using FSH.Modules.Ai.Contracts.v1.Runs;
using FSH.Modules.Ai.Data;
using FSH.Modules.Ai.Domain;
using FSH.Modules.Ai.Jobs;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Ai.Features.v1.Runs.StartAgentRun;

public sealed class StartAgentRunCommandHandler(
    AiDbContext db,
    IJobService jobs,
    ICurrentUser currentUser)
    : ICommandHandler<StartAgentRunCommand, Guid>
{
    public async ValueTask<Guid> Handle(StartAgentRunCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        var tenantId = currentUser.GetTenant() ?? throw new UnauthorizedException("invalid tenant");
        var userId = currentUser.GetUserId();
        if (userId == Guid.Empty)
        {
            throw new UnauthorizedException("no current user");
        }

        var agent = await db.Agents
            .FirstOrDefaultAsync(a => a.Id == command.AgentId, cancellationToken)
            .ConfigureAwait(false)
            ?? throw new NotFoundException($"Agent {command.AgentId} was not found.");

        if (agent.IsArchived)
        {
            throw new CustomException(
                $"Agent '{agent.Name}' is archived and takes no runs.",
                errors: null,
                HttpStatusCode.Conflict);
        }

        if (agent.AccessMode == AgentAccessMode.Selected
            && !agent.AccessUserIds().Contains(userId.ToString(), StringComparer.OrdinalIgnoreCase))
        {
            throw new ForbiddenException();
        }

        var run = AiAgentRun.Create(agent.Id, null, AgentRunTrigger.Manual, command.Input);
        db.AgentRuns.Add(run);
        await db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        jobs.Enqueue<AgentRunJob>(j => j.ExecuteAsync(tenantId, run.Id, CancellationToken.None));

        return run.Id;
    }
}
