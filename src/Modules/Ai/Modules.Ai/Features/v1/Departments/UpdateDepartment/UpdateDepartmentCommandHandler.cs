using System.Net;
using FSH.Framework.Core.Exceptions;
using FSH.Modules.Ai.Contracts.v1.Departments;
using FSH.Modules.Ai.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Ai.Features.v1.Departments.UpdateDepartment;

public sealed class UpdateDepartmentCommandHandler(AiDbContext db)
    : ICommandHandler<UpdateDepartmentCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateDepartmentCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        var department = await db.Departments
            .FirstOrDefaultAsync(d => d.Id == command.Id, cancellationToken)
            .ConfigureAwait(false)
            ?? throw new NotFoundException($"Department {command.Id} was not found.");

        var taken = await db.Departments
            .AnyAsync(d => d.Id != command.Id && d.Name == command.Name.Trim(), cancellationToken)
            .ConfigureAwait(false);
        if (taken)
        {
            throw new CustomException(
                $"A department named '{command.Name.Trim()}' already exists.",
                errors: null,
                HttpStatusCode.Conflict);
        }

        department.Update(command.Name, command.Description);
        await db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return department.Id;
    }
}
