using FSH.Module.Identity.Contracts.DTOs;
using FSH.Module.Identity.Contracts.Services;
using FSH.Module.Identity.Contracts.v1.Users.GetUser;
using Mediator;

namespace FSH.Module.Identity.Features.v1.Users.GetUserById;

public sealed class GetUserByIdQueryHandler(IUserService userService) : IQueryHandler<GetUserQuery, UserDto>
{
    public async ValueTask<UserDto> Handle(GetUserQuery query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);
        return await userService.GetAsync(query.Id, cancellationToken).ConfigureAwait(false);
    }
}
