using FSH.Module.Identity.Contracts.DTOs;
using FSH.Module.Identity.Contracts.Services;
using FSH.Module.Identity.Contracts.v1.Roles.GetRoleWithPermissions;
using Mediator;

namespace FSH.Module.Identity.Features.v1.Roles.GetRoleWithPermissions;

public sealed class GetRoleWithPermissionsQueryHandler(IRoleService roleService)
    : IQueryHandler<GetRoleWithPermissionsQuery, RoleDto>
{
    public async ValueTask<RoleDto> Handle(GetRoleWithPermissionsQuery query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);
        return await roleService.GetWithPermissionsAsync(query.Id, cancellationToken).ConfigureAwait(false);
    }
}
