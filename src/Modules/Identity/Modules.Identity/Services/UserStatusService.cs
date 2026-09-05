using System.Net;
using Finbuckle.MultiTenant.Abstractions;
using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Framework.Shared.Constants;
using FSH.Framework.Shared.Multitenancy;
using FSH.Modules.Auditing.Contracts;
using FSH.Modules.Identity.Contracts.Services;
using FSH.Modules.Identity.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Identity.Services;

internal sealed class UserStatusService(
    UserManager<FshUser> userManager,
    IMultiTenantContextAccessor<AppTenantInfo> multiTenantContextAccessor,
    ICurrentUser currentUser,
    IAuditClient auditClient) : IUserStatusService
{
    // Soft-delete is functionally identical to deactivation — delegate so the same admin/self/last-admin
    // guards and audit pipeline apply uniformly to both DELETE /users/{id} and PATCH /users/{id}.
    public Task DeleteAsync(string userId, CancellationToken cancellationToken = default)
        => ToggleStatusAsync(activateUser: false, userId, cancellationToken);

    public async Task ToggleStatusAsync(bool activateUser, string userId, CancellationToken cancellationToken)
    {
        EnsureValidTenant();

        var context = await BuildToggleContextAsync(userId, activateUser, cancellationToken).ConfigureAwait(false);

        await ValidateTogglePermissionsAsync(context, cancellationToken).ConfigureAwait(false);

        ApplyStatusChange(context);

        await SaveAndAuditAsync(context, cancellationToken).ConfigureAwait(false);
    }

    private void EnsureValidTenant()
    {
        if (string.IsNullOrWhiteSpace(multiTenantContextAccessor?.MultiTenantContext?.TenantInfo?.Id))
        {
            throw new UnauthorizedException("invalid tenant");
        }
    }

    private async Task<ToggleStatusContext> BuildToggleContextAsync(
        string userId,
        bool activateUser,
        CancellationToken cancellationToken)
    {
        var actorId = currentUser.GetUserId();
        if (actorId == Guid.Empty)
        {
            throw new UnauthorizedException("authenticated user required to toggle status");
        }

        var actor = await userManager.FindByIdAsync(actorId.ToString()).ConfigureAwait(false)
            ?? throw new UnauthorizedException("current user not found");

        var targetUser = await userManager.Users
            .Where(u => u.Id == userId)
            .FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false)
            ?? throw new NotFoundException("User Not Found.");

        return new ToggleStatusContext(
            ActorId: actorId,
            Actor: actor,
            TargetUser: targetUser,
            ActivateUser: activateUser,
            TenantId: multiTenantContextAccessor?.MultiTenantContext?.TenantInfo?.Id);
    }

    private async Task ValidateTogglePermissionsAsync(
        ToggleStatusContext context,
        CancellationToken cancellationToken)
    {
        if (!await userManager.IsInRoleAsync(context.Actor, RoleConstants.Admin).ConfigureAwait(false))
        {
            await AuditPolicyFailureAsync(context, "ActorNotAdmin", cancellationToken).ConfigureAwait(false);
            throw new ForbiddenException("Only administrators can change user status.");
        }

        if (!context.ActivateUser && context.ActorId.ToString() == context.TargetUser.Id)
        {
            await AuditPolicyFailureAsync(context, "SelfDeactivationBlocked", cancellationToken).ConfigureAwait(false);
            throw new CustomException("Users cannot deactivate themselves.", Array.Empty<string>(), HttpStatusCode.BadRequest);
        }

        if (!context.ActivateUser && await userManager.IsInRoleAsync(context.TargetUser, RoleConstants.Admin).ConfigureAwait(false))
        {
            await AuditPolicyFailureAsync(context, "AdminDeactivationBlocked", cancellationToken).ConfigureAwait(false);
            throw new CustomException("Administrators cannot be deactivated.", Array.Empty<string>(), HttpStatusCode.BadRequest);
        }

        if (!context.ActivateUser)
        {
            await EnsureMinimumActiveAdminsAsync(context, cancellationToken).ConfigureAwait(false);
        }
    }

    private async Task EnsureMinimumActiveAdminsAsync(
        ToggleStatusContext context,
        CancellationToken cancellationToken)
    {
        var activeAdmins = await userManager.GetUsersInRoleAsync(RoleConstants.Admin).ConfigureAwait(false);
        if (!activeAdmins.Any(u => u.IsActive))
        {
            await AuditPolicyFailureAsync(context, "NoActiveAdmins", cancellationToken).ConfigureAwait(false);
            throw new CustomException("Tenant must have at least one active administrator.", Array.Empty<string>(), HttpStatusCode.BadRequest);
        }
    }

    private static void ApplyStatusChange(ToggleStatusContext context)
    {
        if (context.ActivateUser)
        {
            context.TargetUser.Activate(context.ActorId.ToString(), context.TenantId);
        }
        else
        {
            context.TargetUser.Deactivate(context.ActorId.ToString(), "Status toggled by administrator", context.TenantId);
        }
    }

    private async Task SaveAndAuditAsync(
        ToggleStatusContext context,
        CancellationToken cancellationToken)
    {
        var result = await userManager.UpdateAsync(context.TargetUser).ConfigureAwait(false);
        if (!result.Succeeded)
        {
            throw new CustomException("Toggle status failed", result.Errors.Select(e => e.Description).ToList(), HttpStatusCode.BadRequest);
        }

        await auditClient.WriteActivityAsync(
            ActivityKind.Command,
            name: "ToggleUserStatus",
            statusCode: 204,
            durationMs: 0,
            captured: BodyCapture.None,
            requestSize: 0,
            responseSize: 0,
            requestPreview: new { actorId = context.ActorId.ToString(), targetUserId = context.TargetUser.Id, action = context.ActivateUser ? "activate" : "deactivate", tenant = context.TenantId ?? "unknown" },
            responsePreview: new { outcome = "success" },
            severity: AuditSeverity.Information,
            source: "Identity",
            ct: cancellationToken).ConfigureAwait(false);
    }

    private async Task AuditPolicyFailureAsync(
        ToggleStatusContext context,
        string reason,
        CancellationToken cancellationToken)
    {
        var claims = new Dictionary<string, object?>
        {
            ["actorId"] = context.ActorId.ToString(),
            ["targetUserId"] = context.TargetUser.Id,
            ["tenant"] = context.TenantId ?? "unknown",
            ["action"] = context.ActivateUser ? "activate" : "deactivate"
        };

        await auditClient.WriteSecurityAsync(
            SecurityAction.PolicyFailed,
            subjectId: context.ActorId.ToString(),
            reasonCode: reason,
            claims: claims,
            severity: AuditSeverity.Warning,
            source: "Identity",
            ct: cancellationToken).ConfigureAwait(false);
    }

    private sealed record ToggleStatusContext(
        Guid ActorId,
        FshUser Actor,
        FshUser TargetUser,
        bool ActivateUser,
        string? TenantId);
}