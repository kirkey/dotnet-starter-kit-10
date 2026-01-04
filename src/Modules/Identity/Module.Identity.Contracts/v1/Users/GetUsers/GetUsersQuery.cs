using FSH.Module.Identity.Contracts.DTOs;
using Mediator;

namespace FSH.Module.Identity.Contracts.v1.Users.GetUsers;

public sealed record GetUsersQuery : IQuery<List<UserDto>>;

