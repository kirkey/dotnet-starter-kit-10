using System.Net;
using FSH.Framework.Core.Exceptions;
using FSH.Modules.Ai.Contracts.v1.Agents;
using FSH.Modules.Ai.Data;
using FSH.Modules.Ai.Features.v1.Agents.CreateAgent;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Ai.Features.v1.Agents.UpdateAgent;

public sealed class UpdateAgentCommandHandler(AiDbContext db)
    : ICommandHandler<UpdateAgentCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateAgentCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        var agent = await db.Agents
            .FirstOrDefaultAsync(a => a.Id == command.Id, cancellationToken)
            .ConfigureAwait(false)
            ?? throw new NotFoundException($"Agent {command.Id} was not found.");

        var taken = await db.Agents
            .AnyAsync(a => a.Id != command.Id
                && a.DepartmentId == agent.DepartmentId
                && a.Name == command.Name.Trim(), cancellationToken)
            .ConfigureAwait(false);
        if (taken)
        {
            throw new CustomException(
                $"An agent named '{command.Name.Trim()}' already exists in this department.",
                errors: null,
                HttpStatusCode.Conflict);
        }

        CreateAgentCommandHandler.RejectUnsupportedVariant(command.RuntimeBinding, command.Variant);

        agent.Update(
            command.Name,
            command.Instructions,
            command.Skills,
            command.RuntimeBinding,
            command.Model,
            command.Variant,
            command.AccessMode,
            command.AccessUserIds);

        await db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return agent.Id;
    }
}
