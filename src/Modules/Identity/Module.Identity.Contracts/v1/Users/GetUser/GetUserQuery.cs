using FSH.Module.Identity.Contracts.DTOs;
using Mediator;

namespace FSH.Module.Identity.Contracts.v1.Users.GetUser;

public sealed record GetUserQuery(string Id) : IQuery<UserDto>;

