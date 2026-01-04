using FSH.Module.Identity.Contracts.DTOs;
using FSH.Module.Identity.Contracts.Services;
using FSH.Module.Identity.Contracts.v1.Users.GetUserRoles;
using Mediator;

namespace FSH.Module.Identity.Features.v1.Users.GetUserRoles;

public sealed class GetUserRolesQueryHandler(IUserService userService)
    : IQueryHandler<GetUserRolesQuery, List<UserRoleDto>>
{
    public async ValueTask<List<UserRoleDto>> Handle(GetUserRolesQuery query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);
        return await userService.GetUserRolesAsync(query.UserId, cancellationToken).ConfigureAwait(false);
    }
}
