using FSH.Module.Identity.Contracts.Services;
using FSH.Module.Identity.Contracts.v1.Users.GetUserPermissions;
using Mediator;

namespace FSH.Module.Identity.Features.v1.Users.GetUserPermissions;

public sealed class GetCurrentUserPermissionsQueryHandler(IUserService userService)
    : IQueryHandler<GetCurrentUserPermissionsQuery, List<string>?>
{
    public async ValueTask<List<string>?> Handle(GetCurrentUserPermissionsQuery query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);
        return await userService.GetPermissionsAsync(query.UserId, cancellationToken).ConfigureAwait(false);
    }
}
