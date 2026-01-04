using FSH.Module.Identity.Contracts.DTOs;
using FSH.Module.Identity.Contracts.Services;
using FSH.Module.Identity.Contracts.v1.Users.GetUsers;
using Mediator;

namespace FSH.Module.Identity.Features.v1.Users.GetUsers;

public sealed class GetUsersQueryHandler(IUserService userService) : IQueryHandler<GetUsersQuery, List<UserDto>>
{
    public async ValueTask<List<UserDto>> Handle(GetUsersQuery query, CancellationToken cancellationToken)
    {
        return await userService.GetListAsync(cancellationToken).ConfigureAwait(false);
    }
}
