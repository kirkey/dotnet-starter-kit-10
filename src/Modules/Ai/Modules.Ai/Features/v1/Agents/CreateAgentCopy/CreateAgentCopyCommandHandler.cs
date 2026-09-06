using System.Net;
using FSH.Framework.Core.Exceptions;
using FSH.Modules.Ai.Contracts.v1.Agents;
using FSH.Modules.Ai.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Ai.Features.v1.Agents.CreateAgentCopy;

/// <summary>Copies an agent's configuration under a new name. Configuration only, never credentials.</summary>
public sealed class CreateAgentCopyCommandHandler(AiDbContext db)
    : ICommandHandler<CreateAgentCopyCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateAgentCopyCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        var agent = await db.Agents
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == command.Id, cancellationToken)
            .ConfigureAwait(false)
            ?? throw new NotFoundException($"Agent {command.Id} was not found.");

        var taken = await db.Agents
            .AnyAsync(a => a.DepartmentId == agent.DepartmentId && a.Name == command.Name.Trim(), cancellationToken)
            .ConfigureAwait(false);
        if (taken)
        {
            throw new CustomException(
                $"An agent named '{command.Name.Trim()}' already exists in this department.",
                errors: null,
                HttpStatusCode.Conflict);
        }

        var copy = agent.Duplicate(command.Name);
        db.Agents.Add(copy);
        await db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return copy.Id;
    }
}
