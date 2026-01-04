using FSH.Module.Identity.Contracts.DTOs;
using Mediator;

namespace FSH.Module.Identity.Contracts.v1.Roles.GetRoleWithPermissions;

public sealed record GetRoleWithPermissionsQuery(string Id) : IQuery<RoleDto>;

