using System.Net;
using FSH.Framework.Core.Exceptions;
using FSH.Modules.Ai.Contracts.Dtos;
using FSH.Modules.Ai.Contracts.v1.Departments;
using FSH.Modules.Ai.Data;
using FSH.Modules.Ai.Domain;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Ai.Features.v1.Departments.CreateDepartment;

public sealed class CreateDepartmentCommandHandler(AiDbContext db)
    : ICommandHandler<CreateDepartmentCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateDepartmentCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        var taken = await db.Departments
            .AnyAsync(d => d.Name == command.Name.Trim(), cancellationToken)
            .ConfigureAwait(false);
        if (taken)
        {
            throw new CustomException(
                $"A department named '{command.Name.Trim()}' already exists.",
                errors: null,
                HttpStatusCode.Conflict);
        }

        var department = AiDepartment.Create(command.Name, command.Description);
        db.Departments.Add(department);
        await db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return department.Id;
    }
}
