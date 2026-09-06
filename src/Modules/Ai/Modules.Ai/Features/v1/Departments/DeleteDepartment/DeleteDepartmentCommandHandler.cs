using System.Net;
using FSH.Framework.Core.Exceptions;
using FSH.Modules.Ai.Contracts.v1.Departments;
using FSH.Modules.Ai.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Ai.Features.v1.Departments.DeleteDepartment;

public sealed class DeleteDepartmentCommandHandler(AiDbContext db)
    : ICommandHandler<DeleteDepartmentCommand, Guid>
{
    public async ValueTask<Guid> Handle(DeleteDepartmentCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        var department = await db.Departments
            .FirstOrDefaultAsync(d => d.Id == command.Id, cancellationToken)
            .ConfigureAwait(false)
            ?? throw new NotFoundException($"Department {command.Id} was not found.");

        var agentCount = await db.Agents
            .CountAsync(a => a.DepartmentId == department.Id, cancellationToken)
            .ConfigureAwait(false);
        if (agentCount > 0)
        {
            throw new CustomException(
                $"Department '{department.Name}' cannot be deleted while it still owns {agentCount} agent(s).",
                errors: null,
                HttpStatusCode.Conflict);
        }

        db.Departments.Remove(department);
        await db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return department.Id;
    }
}
