using FSH.Framework.Caching;
using FSH.Framework.Core.Exceptions;
using FSH.Framework.Shared.Identity;
using FSH.Modules.Identity.Features.v1.Roles;
using FSH.Modules.Identity.Features.v1.Users;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Identity.Services;

internal sealed partial class UserService
{
    public async Task<List<string>?> GetPermissionsAsync(string userId, CancellationToken cancellationToken)
    {
        List<string>? permissions = await cache.GetOrSetAsync(
            GetPermissionCacheKey(userId),
            async () =>
            {
                FshUser? user = await userManager.FindByIdAsync(userId);

                _ = user ?? throw new UnauthorizedException();

                IList<string> userRoles = await userManager.GetRolesAsync(user);
                List<string> permissions = new();
                foreach (FshRole role in await roleManager.Roles
                    .Where(r => userRoles.Contains(r.Name!))
                    .ToListAsync(cancellationToken))
                {
                    permissions.AddRange(await db.RoleClaims
                        .Where(rc => rc.RoleId == role.Id && rc.ClaimType == ClaimConstants.Permission)
                        .Select(rc => rc.ClaimValue!)
                        .ToListAsync(cancellationToken));
                }
                return permissions.Distinct().ToList();
            },
            cancellationToken: cancellationToken);

        return permissions;
    }

    public static string GetPermissionCacheKey(string userId)
    {
        return $"perm:{userId}";
    }

    public async Task<bool> HasPermissionAsync(string userId, string permission, CancellationToken cancellationToken = default)
    {
        List<string>? permissions = await GetPermissionsAsync(userId, cancellationToken);

        return permissions?.Contains(permission) ?? false;
    }

    public Task InvalidatePermissionCacheAsync(string userId, CancellationToken cancellationToken)
    {
        return cache.RemoveItemAsync(GetPermissionCacheKey(userId), cancellationToken);
    }
}