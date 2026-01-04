using FSH.Module.Identity.Contracts.DTOs;
using FSH.Module.Identity.Contracts.Services;
using FSH.Module.Identity.Contracts.v1.Roles.GetRole;
using Mediator;

namespace FSH.Module.Identity.Features.v1.Roles.GetRoleById;

public sealed class GetRoleByIdQueryHandler(IRoleService roleService) : IQueryHandler<GetRoleQuery, RoleDto?>
{
    public async ValueTask<RoleDto?> Handle(GetRoleQuery query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);
        return await roleService.GetRoleAsync(query.Id).ConfigureAwait(false);
    }
}
