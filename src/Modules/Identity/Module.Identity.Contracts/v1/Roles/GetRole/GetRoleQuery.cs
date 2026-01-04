using FSH.Module.Identity.Contracts.DTOs;
using Mediator;

namespace FSH.Module.Identity.Contracts.v1.Roles.GetRole;

public sealed record GetRoleQuery(string Id) : IQuery<RoleDto?>;

