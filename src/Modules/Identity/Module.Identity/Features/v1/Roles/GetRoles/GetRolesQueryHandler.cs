using FSH.Module.Identity.Contracts.DTOs;
using FSH.Module.Identity.Contracts.Services;
using FSH.Module.Identity.Contracts.v1.Roles.GetRoles;
using Mediator;

namespace FSH.Module.Identity.Features.v1.Roles.GetRoles;

public sealed class GetRolesQueryHandler(IRoleService roleService) : IQueryHandler<GetRolesQuery, IEnumerable<RoleDto>>
{
    public async ValueTask<IEnumerable<RoleDto>> Handle(GetRolesQuery query, CancellationToken cancellationToken)
    {
        return await roleService.GetRolesAsync().ConfigureAwait(false);
    }
}
