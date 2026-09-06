using System.Net;
using FSH.Framework.Core.Exceptions;
using FSH.Modules.Ai.Contracts.Dtos;
using FSH.Modules.Ai.Contracts.v1.Agents;
using FSH.Modules.Ai.Data;
using FSH.Modules.Ai.Domain;
using FSH.Modules.Ai.Services;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Ai.Features.v1.Agents.CreateAgent;

public sealed class CreateAgentCommandHandler(AiDbContext db)
    : ICommandHandler<CreateAgentCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateAgentCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        var departmentExists = await db.Departments
            .AnyAsync(d => d.Id == command.DepartmentId, cancellationToken)
            .ConfigureAwait(false);
        if (!departmentExists)
        {
            throw new NotFoundException($"Department {command.DepartmentId} was not found.");
        }

        var taken = await db.Agents
            .AnyAsync(a => a.DepartmentId == command.DepartmentId && a.Name == command.Name.Trim(), cancellationToken)
            .ConfigureAwait(false);
        if (taken)
        {
            throw new CustomException(
                $"An agent named '{command.Name.Trim()}' already exists in this department.",
                errors: null,
                HttpStatusCode.Conflict);
        }

        RejectUnsupportedVariant(command.RuntimeBinding, command.Variant);

        var agent = AiAgent.Create(
            command.DepartmentId,
            command.Name,
            command.Instructions,
            command.Skills,
            command.RuntimeBinding,
            command.Model,
            command.Variant,
            command.AccessMode,
            command.AccessUserIds);

        db.Agents.Add(agent);
        await db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return agent.Id;
    }

    internal static void RejectUnsupportedVariant(string runtimeBinding, AiVariant variant)
    {
        if (!RuntimeVariantMatrix.IsSupported(runtimeBinding, variant))
        {
            var supported = string.Join(", ", RuntimeVariantMatrix.SupportedTiers(runtimeBinding));
            throw new CustomException(
                $"Variant '{variant}' is not supported by runtime '{runtimeBinding}'. Supported: {supported}.",
                errors: null,
                HttpStatusCode.BadRequest);
        }
    }
}
