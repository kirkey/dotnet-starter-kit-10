using FSH.Module.Identity.Contracts.DTOs;
using Mediator;

namespace FSH.Module.Identity.Contracts.v1.Roles.GetRoles;

public sealed record GetRolesQuery : IQuery<IEnumerable<RoleDto>>;

