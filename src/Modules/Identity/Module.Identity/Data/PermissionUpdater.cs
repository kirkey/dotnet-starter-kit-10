using Finbuckle.MultiTenant.Abstractions;
using FSH.Framework.Persistence;
using FSH.Framework.Shared.Identity;
using FSH.Framework.Shared.Multitenancy;
using FSH.Module.Identity.Data;
using FSH.Module.Identity.Features.v1.RoleClaims;
using FSH.Module.Identity.Features.v1.Roles;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace FSH.Module.Identity.Data;

/// <summary>
/// Updates existing roles with new permissions that were added after initial seeding
/// </summary>
internal sealed class PermissionUpdater(
    ILogger<PermissionUpdater> logger,
    IdentityDbContext context,
    RoleManager<FshRole> roleManager,
    TimeProvider timeProvider,
    IMultiTenantContextAccessor<AppTenantInfo> multiTenantContextAccessor) : IDbInitializer
{
    public Task MigrateAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    public async Task SeedAsync(CancellationToken cancellationToken)
    {
        await UpdateRolePermissionsAsync();
    }

    private async Task UpdateRolePermissionsAsync()
    {
        // Get Admin role
        var adminRole = await roleManager.Roles.SingleOrDefaultAsync(r => r.Name == RoleConstants.Admin);
        if (adminRole == null)
        {
            logger.LogWarning("Admin role not found for tenant {TenantId}", multiTenantContextAccessor.MultiTenantContext.TenantInfo?.Id);
            return;
        }

        // Update Admin role with all Admin permissions
        await AssignPermissionsToRoleAsync(context, PermissionConstants.Admin, adminRole);

        // If root tenant, also add Root permissions
        if (multiTenantContextAccessor.MultiTenantContext.TenantInfo?.Id == "root")
        {
            await AssignPermissionsToRoleAsync(context, PermissionConstants.Root, adminRole);
        }

        // Get Basic role
        var basicRole = await roleManager.Roles.SingleOrDefaultAsync(r => r.Name == RoleConstants.Basic);
        if (basicRole != null)
        {
            await AssignPermissionsToRoleAsync(context, PermissionConstants.Basic, basicRole);
        }

        logger.LogInformation("Updated role permissions for tenant {TenantId}", multiTenantContextAccessor.MultiTenantContext.TenantInfo?.Id);
    }

    private async Task AssignPermissionsToRoleAsync(IdentityDbContext dbContext, IReadOnlyList<FshPermission> permissions, FshRole role)
    {
        IList<Claim> currentClaims = await roleManager.GetClaimsAsync(role);
        List<FshRoleClaim> newClaims = permissions
            .Where(permission => !currentClaims.Any(c => c.Type == ClaimConstants.Permission && c.Value == permission.Name))
            .Select(permission => new FshRoleClaim
            {
                RoleId = role.Id,
                ClaimType = ClaimConstants.Permission,
                ClaimValue = permission.Name,
                CreatedBy = "PermissionUpdater",
                CreatedOn = timeProvider.GetUtcNow()
            })
            .ToList();

        if (newClaims.Count == 0)
        {
            return;
        }

        foreach (FshRoleClaim claim in newClaims)
        {
            logger.LogInformation("Adding missing {Role} Permission '{Permission}' for '{TenantId}' Tenant.", 
                role.Name, claim.ClaimValue, multiTenantContextAccessor.MultiTenantContext.TenantInfo?.Id);
            await dbContext.RoleClaims.AddAsync(claim);
        }

        await dbContext.SaveChangesAsync();
    }
}
