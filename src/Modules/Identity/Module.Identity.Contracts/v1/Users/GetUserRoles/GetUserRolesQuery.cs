using FSH.Module.Identity.Contracts.DTOs;
using Mediator;

namespace FSH.Module.Identity.Contracts.v1.Users.GetUserRoles;

public sealed record GetUserRolesQuery(string UserId) : IQuery<List<UserRoleDto>>;

