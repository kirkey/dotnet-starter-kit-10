using FSH.Module.Identity.Contracts.DTOs;
using FSH.Module.Identity.Contracts.Services;
using FSH.Module.Identity.Contracts.v1.Users.GetUserProfile;
using Mediator;

namespace FSH.Module.Identity.Features.v1.Users.GetUserProfile;

public sealed class GetCurrentUserProfileQueryHandler(IUserService userService)
    : IQueryHandler<GetCurrentUserProfileQuery, UserDto>
{
    public async ValueTask<UserDto> Handle(GetCurrentUserProfileQuery query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);
        return await userService.GetAsync(query.UserId, cancellationToken).ConfigureAwait(false);
    }
}
