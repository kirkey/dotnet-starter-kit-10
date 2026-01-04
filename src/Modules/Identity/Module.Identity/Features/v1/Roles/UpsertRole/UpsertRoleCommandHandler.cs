using FSH.Module.Identity.Contracts.DTOs;
using FSH.Module.Identity.Contracts.Services;
using FSH.Module.Identity.Contracts.v1.Roles.UpsertRole;
using Mediator;

namespace FSH.Module.Identity.Features.v1.Roles.UpsertRole;

public sealed class UpsertRoleCommandHandler(IRoleService roleService) : ICommandHandler<UpsertRoleCommand, RoleDto>
{
    public async ValueTask<RoleDto> Handle(UpsertRoleCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        return await roleService.CreateOrUpdateRoleAsync(command.Id, command.Name, command.Description ?? string.Empty)
            .ConfigureAwait(false);
    }
}
