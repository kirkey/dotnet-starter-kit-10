using Finbuckle.MultiTenant.Abstractions;
using FSH.Framework.Core.Context;
using FSH.Framework.Shared.Multitenancy;
using FSH.Module.Identity.Contracts.DTOs;
using FSH.Module.Identity.Contracts.Services;
using FSH.Module.Identity.Data;
using FSH.Module.Identity.Features.v1.Sessions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UAParser;

namespace FSH.Module.Identity.Services;

public sealed class SessionService(
    IdentityDbContext db,
    ICurrentUser currentUser,
    IMultiTenantContextAccessor<AppTenantInfo> multiTenantContextAccessor,
    ILogger<SessionService> logger)
    : ISessionService
{
    private readonly Parser _uaParser = Parser.GetDefault();

    private void EnsureValidTenant()
    {
        if (string.IsNullOrWhiteSpace(multiTenantContextAccessor?.MultiTenantContext?.TenantInfo?.Id))
        {
            throw new UnauthorizedAccessException("Invalid tenant");
        }
    }

    public async Task<UserSessionDto> CreateSessionAsync(
        string userId,
        string refreshTokenHash,
        string ipAddress,
        string userAgent,
        DateTime expiresAt,
        CancellationToken cancellationToken = default)
    {
        EnsureValidTenant();

        ClientInfo? clientInfo = _uaParser.Parse(userAgent);

        UserSession session = new()
        {
            UserId = userId,
            RefreshTokenHash = refreshTokenHash,
            IpAddress = ipAddress,
            UserAgent = userAgent,
            DeviceType = GetDeviceType(clientInfo.Device.Family),
            Browser = clientInfo.UA.Family,
            BrowserVersion = clientInfo.UA.Major,
            OperatingSystem = clientInfo.OS.Family,
            OsVersion = clientInfo.OS.Major,
            ExpiresAt = expiresAt,
            CreatedAt = DateTime.UtcNow,
            LastActivityAt = DateTime.UtcNow
        };

        db.UserSessions.Add(session);
        await db.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Created session {SessionId} for user {UserId}", session.Id, userId);

        return MapToDto(session, isCurrentSession: true);
    }

    public async Task<List<UserSessionDto>> GetUserSessionsAsync(
        string userId,
        CancellationToken cancellationToken = default)
    {
        EnsureValidTenant();

        string currentUserId = currentUser.GetUserId().ToString();
        if (!string.Equals(userId, currentUserId, StringComparison.OrdinalIgnoreCase))
        {
            throw new UnauthorizedAccessException("Cannot view sessions for another user");
        }

        List<UserSession> sessions = await db.UserSessions
            .AsNoTracking()
            .Where(s => s.UserId == userId && !s.IsRevoked && s.ExpiresAt > DateTime.UtcNow)
            .OrderByDescending(s => s.LastActivityAt)
            .ToListAsync(cancellationToken);

        return sessions.Select(s => MapToDto(s, isCurrentSession: false)).ToList();
    }

    public async Task<List<UserSessionDto>> GetUserSessionsForAdminAsync(
        string userId,
        CancellationToken cancellationToken = default)
    {
        EnsureValidTenant();

        List<UserSession> sessions = await db.UserSessions
            .AsNoTracking()
            .Include(s => s.User)
            .Where(s => s.UserId == userId && !s.IsRevoked && s.ExpiresAt > DateTime.UtcNow)
            .OrderByDescending(s => s.LastActivityAt)
            .ToListAsync(cancellationToken);

        return sessions.Select(s => MapToDto(s, isCurrentSession: false)).ToList();
    }

    public async Task<UserSessionDto?> GetSessionAsync(
        Guid sessionId,
        CancellationToken cancellationToken = default)
    {
        EnsureValidTenant();

        UserSession? session = await db.UserSessions
            .AsNoTracking()
            .Include(s => s.User)
            .FirstOrDefaultAsync(s => s.Id == sessionId, cancellationToken);

        return session is null ? null : MapToDto(session, isCurrentSession: false);
    }

    public async Task<bool> RevokeSessionAsync(
        Guid sessionId,
        string revokedBy,
        string? reason = null,
        CancellationToken cancellationToken = default)
    {
        EnsureValidTenant();

        UserSession? session = await db.UserSessions
            .FirstOrDefaultAsync(s => s.Id == sessionId && !s.IsRevoked, cancellationToken);

        if (session is null)
        {
            return false;
        }

        string currentUserId = currentUser.GetUserId().ToString();
        if (!string.Equals(session.UserId, currentUserId, StringComparison.OrdinalIgnoreCase))
        {
            throw new UnauthorizedAccessException("Cannot revoke session for another user");
        }

        session.IsRevoked = true;
        session.RevokedAt = DateTime.UtcNow;
        session.RevokedBy = revokedBy;
        session.RevokedReason = reason ?? "User requested";

        await db.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Session {SessionId} revoked by {RevokedBy}", sessionId, revokedBy);

        return true;
    }

    public async Task<int> RevokeAllSessionsAsync(
        string userId,
        string revokedBy,
        Guid? exceptSessionId = null,
        string? reason = null,
        CancellationToken cancellationToken = default)
    {
        EnsureValidTenant();

        string currentUserId = currentUser.GetUserId().ToString();
        if (!string.Equals(userId, currentUserId, StringComparison.OrdinalIgnoreCase))
        {
            throw new UnauthorizedAccessException("Cannot revoke sessions for another user");
        }

        IQueryable<UserSession> query = db.UserSessions
            .Where(s => s.UserId == userId && !s.IsRevoked);

        if (exceptSessionId.HasValue)
        {
            query = query.Where(s => s.Id != exceptSessionId.Value);
        }

        List<UserSession> sessions = await query.ToListAsync(cancellationToken);

        foreach (UserSession session in sessions)
        {
            session.IsRevoked = true;
            session.RevokedAt = DateTime.UtcNow;
            session.RevokedBy = revokedBy;
            session.RevokedReason = reason ?? "User requested logout from all devices";
        }

        await db.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Revoked {Count} sessions for user {UserId}", sessions.Count, userId);

        return sessions.Count;
    }

    public async Task<int> RevokeAllSessionsForAdminAsync(
        string userId,
        string revokedBy,
        string? reason = null,
        CancellationToken cancellationToken = default)
    {
        EnsureValidTenant();

        List<UserSession> sessions = await db.UserSessions
            .Where(s => s.UserId == userId && !s.IsRevoked)
            .ToListAsync(cancellationToken);

        foreach (UserSession session in sessions)
        {
            session.IsRevoked = true;
            session.RevokedAt = DateTime.UtcNow;
            session.RevokedBy = revokedBy;
            session.RevokedReason = reason ?? "Admin requested";
        }

        await db.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Admin {AdminId} revoked {Count} sessions for user {UserId}",
            revokedBy, sessions.Count, userId);

        return sessions.Count;
    }

    public async Task<bool> RevokeSessionForAdminAsync(
        Guid sessionId,
        string revokedBy,
        string? reason = null,
        CancellationToken cancellationToken = default)
    {
        EnsureValidTenant();

        UserSession? session = await db.UserSessions
            .FirstOrDefaultAsync(s => s.Id == sessionId && !s.IsRevoked, cancellationToken);

        if (session is null)
        {
            return false;
        }

        session.IsRevoked = true;
        session.RevokedAt = DateTime.UtcNow;
        session.RevokedBy = revokedBy;
        session.RevokedReason = reason ?? "Admin requested";

        await db.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Admin {AdminId} revoked session {SessionId}", revokedBy, sessionId);

        return true;
    }

    public async Task UpdateSessionActivityAsync(
        string refreshTokenHash,
        CancellationToken cancellationToken = default)
    {
        EnsureValidTenant();

        UserSession? session = await db.UserSessions
            .FirstOrDefaultAsync(s => s.RefreshTokenHash == refreshTokenHash && !s.IsRevoked, cancellationToken);

        if (session is not null)
        {
            session.LastActivityAt = DateTime.UtcNow;
            await db.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task UpdateSessionRefreshTokenAsync(
        string oldRefreshTokenHash,
        string newRefreshTokenHash,
        DateTime newExpiresAt,
        CancellationToken cancellationToken = default)
    {
        EnsureValidTenant();

        UserSession? session = await db.UserSessions
            .FirstOrDefaultAsync(s => s.RefreshTokenHash == oldRefreshTokenHash && !s.IsRevoked, cancellationToken);

        if (session is not null)
        {
            session.RefreshTokenHash = newRefreshTokenHash;
            session.ExpiresAt = newExpiresAt;
            session.LastActivityAt = DateTime.UtcNow;
            await db.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Updated session {SessionId} with new refresh token", session.Id);
        }
    }

    public async Task<bool> ValidateSessionAsync(
        string refreshTokenHash,
        CancellationToken cancellationToken = default)
    {
        EnsureValidTenant();

        UserSession? session = await db.UserSessions
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.RefreshTokenHash == refreshTokenHash, cancellationToken);

        if (session is null)
        {
            return true; // No session tracking for this token (backwards compatibility)
        }

        return !session.IsRevoked && session.ExpiresAt > DateTime.UtcNow;
    }

    public async Task<Guid?> GetSessionIdByRefreshTokenAsync(
        string refreshTokenHash,
        CancellationToken cancellationToken = default)
    {
        EnsureValidTenant();

        UserSession? session = await db.UserSessions
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.RefreshTokenHash == refreshTokenHash && !s.IsRevoked, cancellationToken);

        return session?.Id;
    }

    public async Task CleanupExpiredSessionsAsync(
        CancellationToken cancellationToken = default)
    {
        DateTime cutoffDate = DateTime.UtcNow.AddDays(-30); // Keep revoked sessions for 30 days for audit
        List<UserSession> expiredSessions = await db.UserSessions
            .Where(s => s.ExpiresAt < DateTime.UtcNow && s.ExpiresAt < cutoffDate)
            .ToListAsync(cancellationToken);

        if (expiredSessions.Count > 0)
        {
            db.UserSessions.RemoveRange(expiredSessions);
            await db.SaveChangesAsync(cancellationToken);
            logger.LogInformation("Cleaned up {Count} expired sessions", expiredSessions.Count);
        }
    }

    private static string GetDeviceType(string deviceFamily)
    {
        if (string.IsNullOrWhiteSpace(deviceFamily) || deviceFamily == "Other")
        {
            return "Desktop";
        }

        string lower = deviceFamily.ToLowerInvariant();
        if (lower.Contains("mobile") || lower.Contains("phone") || lower.Contains("iphone") || lower.Contains("android"))
        {
            return "Mobile";
        }

        if (lower.Contains("tablet") || lower.Contains("ipad"))
        {
            return "Tablet";
        }

        return "Desktop";
    }

    private static UserSessionDto MapToDto(UserSession session, bool isCurrentSession)
    {
        return new UserSessionDto
        {
            Id = session.Id,
            UserId = session.UserId,
            UserName = session.User?.UserName,
            UserEmail = session.User?.Email,
            IpAddress = session.IpAddress,
            DeviceType = session.DeviceType,
            Browser = session.Browser,
            BrowserVersion = session.BrowserVersion,
            OperatingSystem = session.OperatingSystem,
            OsVersion = session.OsVersion,
            CreatedAt = session.CreatedAt,
            LastActivityAt = session.LastActivityAt,
            ExpiresAt = session.ExpiresAt,
            IsActive = !session.IsRevoked && session.ExpiresAt > DateTime.UtcNow,
            IsCurrentSession = isCurrentSession
        };
    }
}
