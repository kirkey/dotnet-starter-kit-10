using FSH.Module.Identity.Contracts.DTOs;
using Mediator;

namespace FSH.Module.Identity.Contracts.v1.Users.GetUserProfile;

public sealed record GetCurrentUserProfileQuery(string UserId) : IQuery<UserDto>;

