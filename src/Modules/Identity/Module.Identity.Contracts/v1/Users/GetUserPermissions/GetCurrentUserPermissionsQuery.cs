using Mediator;

namespace FSH.Module.Identity.Contracts.v1.Users.GetUserPermissions;

public sealed record GetCurrentUserPermissionsQuery(string UserId) : IQuery<List<string>?>;

