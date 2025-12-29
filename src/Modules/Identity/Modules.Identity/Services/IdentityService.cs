using Finbuckle.MultiTenant.Abstractions;
using FSH.Framework.Core.Exceptions;
using FSH.Framework.Shared.Identity;
using FSH.Framework.Shared.Multitenancy;
using FSH.Modules.Identity.Contracts.Services;
using FSH.Modules.Identity.Features.v1.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace FSH.Modules.Identity.Services;

public sealed class IdentityService(
    UserManager<FshUser> userManager,
    IMultiTenantContextAccessor<AppTenantInfo>? multiTenantContextAccessor,
    ILogger<IdentityService> logger)
    : IIdentityService
{
    public async Task<(string Subject, IEnumerable<Claim> Claims)?>
        ValidateCredentialsAsync(string email, string password, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(email);
        ArgumentNullException.ThrowIfNull(password);

        AppTenantInfo? currentTenant = multiTenantContextAccessor!.MultiTenantContext.TenantInfo;
        if (currentTenant == null) throw new UnauthorizedException();

        if (string.IsNullOrWhiteSpace(currentTenant.Id)
           || await userManager.FindByEmailAsync(email.Trim().Normalize()) is not { } user
           || !await userManager.CheckPasswordAsync(user, password))
        {
            throw new UnauthorizedException();
        }

        if (!user.IsActive)
        {
            throw new UnauthorizedException("user is deactivated");
        }

        if (!user.EmailConfirmed)
        {
            throw new UnauthorizedException("email not confirmed");
        }

        if (currentTenant.Id != MultitenancyConstants.Root.Id)
        {
            if (!currentTenant.IsActive)
            {
                throw new UnauthorizedException($"tenant {currentTenant.Id} is deactivated");
            }

            if (DateTime.UtcNow > currentTenant.ValidUpto)
            {
                throw new UnauthorizedException($"tenant {currentTenant.Id} validity has expired");
            }
        }

        // Build user claims
        List<Claim> claims = new()
        {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Email, user.Email!),
            new(ClaimTypes.Name, user.FirstName ?? string.Empty),
            new(ClaimTypes.MobilePhone, user.PhoneNumber ?? string.Empty),
            new(ClaimConstants.Fullname, $"{user.FirstName} {user.LastName}"),
            new(ClaimTypes.Surname, user.LastName ?? string.Empty),
            new(ClaimConstants.Tenant, multiTenantContextAccessor!.MultiTenantContext.TenantInfo!.Id),
            new(ClaimConstants.ImageUrl, user.ImageUrl == null ? string.Empty : user.ImageUrl.ToString())
        };

        // Add roles as claims
        IList<string> roles = await userManager.GetRolesAsync(user);
        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

        return (user.Id, claims);
    }

    public async Task<(string Subject, IEnumerable<Claim> Claims)?>
        ValidateRefreshTokenAsync(string refreshToken, CancellationToken ct = default)
    {
        AppTenantInfo? currentTenant = multiTenantContextAccessor!.MultiTenantContext.TenantInfo;
        if (currentTenant == null) throw new UnauthorizedException();

        if (string.IsNullOrWhiteSpace(currentTenant.Id))
        {
            throw new UnauthorizedException();
        }

        string hashedToken = HashToken(refreshToken);

        logger.LogDebug(
            "Validating refresh token for tenant {TenantId}. Token hash: {TokenHash}",
            currentTenant.Id,
            hashedToken[..Math.Min(8, hashedToken.Length)] + "...");

        FshUser? user = await userManager.Users
            .FirstOrDefaultAsync(u => u.RefreshToken == hashedToken, ct);

        if (user is null)
        {
            logger.LogWarning(
                "No user found with matching refresh token hash for tenant {TenantId}",
                currentTenant.Id);
            throw new UnauthorizedException("refresh token is invalid or expired");
        }

        if (user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            logger.LogWarning(
                "Refresh token expired for user {UserId}. Expired at: {ExpiryTime}, Current time: {CurrentTime}",
                user.Id,
                user.RefreshTokenExpiryTime,
                DateTime.UtcNow);
            throw new UnauthorizedException("refresh token is invalid or expired");
        }

        if (!user.IsActive)
        {
            throw new UnauthorizedException("user is deactivated");
        }

        if (!user.EmailConfirmed)
        {
            throw new UnauthorizedException("email not confirmed");
        }

        if (currentTenant.Id != MultitenancyConstants.Root.Id)
        {
            if (!currentTenant.IsActive)
            {
                throw new UnauthorizedException($"tenant {currentTenant.Id} is deactivated");
            }

            if (DateTime.UtcNow > currentTenant.ValidUpto)
            {
                throw new UnauthorizedException($"tenant {currentTenant.Id} validity has expired");
            }
        }

        List<Claim> claims = new()
        {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Email, user.Email!),
            new(ClaimTypes.Name, user.FirstName ?? string.Empty),
            new(ClaimTypes.MobilePhone, user.PhoneNumber ?? string.Empty),
            new(ClaimConstants.Fullname, $"{user.FirstName} {user.LastName}"),
            new(ClaimTypes.Surname, user.LastName ?? string.Empty),
            new(ClaimConstants.Tenant, multiTenantContextAccessor!.MultiTenantContext.TenantInfo!.Id),
            new(ClaimConstants.ImageUrl, user.ImageUrl == null ? string.Empty : user.ImageUrl.ToString())
        };

        IList<string> roles = await userManager.GetRolesAsync(user);
        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

        return (user.Id, claims);
    }

    public async Task StoreRefreshTokenAsync(string subject, string refreshToken, DateTime expiresAtUtc, CancellationToken ct = default)
    {
        AppTenantInfo? currentTenant = multiTenantContextAccessor!.MultiTenantContext.TenantInfo;
        if (currentTenant == null) throw new UnauthorizedException();

        if (string.IsNullOrWhiteSpace(currentTenant.Id))
        {
            throw new UnauthorizedException();
        }

        FshUser? user = await userManager.FindByIdAsync(subject);

        if (user is null)
        {
            throw new UnauthorizedException("user not found");
        }

        string hashedToken = HashToken(refreshToken);
        user.RefreshToken = hashedToken;
        user.RefreshTokenExpiryTime = expiresAtUtc;

        logger.LogDebug(
            "Storing refresh token for user {UserId} in tenant {TenantId}. Token hash: {TokenHash}, Expires: {ExpiresAt}",
            subject,
            currentTenant.Id,
            hashedToken[..Math.Min(8, hashedToken.Length)] + "...",
            expiresAtUtc);

        IdentityResult result = await userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            logger.LogError("Failed to persist refresh token for user {UserId}: {Errors}", subject, string.Join(", ", result.Errors.Select(e => e.Description)));
            throw new UnauthorizedException("could not persist refresh token");
        }
    }

    private static string HashToken(string token)
    {
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(token);
        byte[] hash = System.Security.Cryptography.SHA256.HashData(bytes);
        return Convert.ToBase64String(hash);
    }
}
