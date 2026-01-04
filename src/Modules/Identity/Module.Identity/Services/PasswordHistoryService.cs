using FSH.Module.Identity.Data;
using FSH.Module.Identity.Features.v1.Users;
using FSH.Module.Identity.Features.v1.Users.PasswordHistory;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace FSH.Module.Identity.Services;

public interface IPasswordHistoryService
{
    Task<bool> IsPasswordInHistoryAsync(FshUser user, string newPassword, CancellationToken cancellationToken = default);
    Task SavePasswordHistoryAsync(FshUser user, CancellationToken cancellationToken = default);
    Task CleanupOldPasswordHistoryAsync(string userId, CancellationToken cancellationToken = default);
}

internal sealed class PasswordHistoryService(
    IdentityDbContext db,
    UserManager<FshUser> userManager,
    IOptions<PasswordPolicyOptions> passwordPolicyOptions)
    : IPasswordHistoryService
{
    private readonly PasswordPolicyOptions _passwordPolicyOptions = passwordPolicyOptions.Value;

    public async Task<bool> IsPasswordInHistoryAsync(FshUser user, string newPassword, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(user);
        ArgumentNullException.ThrowIfNull(newPassword);

        // Get the last N passwords from history (where N = PasswordHistoryCount)
        int passwordHistoryCount = _passwordPolicyOptions.PasswordHistoryCount;
        if (passwordHistoryCount <= 0)
        {
            return false; // Password history check disabled
        }

        List<string> recentPasswordHashes = await db.Set<PasswordHistory>()
            .Where(ph => ph.UserId == user.Id)
            .OrderByDescending(ph => ph.CreatedAt)
            .Take(passwordHistoryCount)
            .Select(ph => ph.PasswordHash)
            .ToListAsync(cancellationToken);

        // Check if the new password matches any recent password
        foreach (string passwordHash in recentPasswordHashes)
        {
            IPasswordHasher<FshUser> passwordHasher = userManager.PasswordHasher;
            PasswordVerificationResult result = passwordHasher.VerifyHashedPassword(user, passwordHash, newPassword);

            if (result == PasswordVerificationResult.Success || result == PasswordVerificationResult.SuccessRehashNeeded)
            {
                return true; // Password is in history
            }
        }

        return false; // Password is not in history
    }

    public async Task SavePasswordHistoryAsync(FshUser user, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(user);

        PasswordHistory passwordHistoryEntry = new()
        {
            UserId = user.Id,
            PasswordHash = user.PasswordHash!,
            CreatedAt = DateTime.UtcNow
        };

        db.Set<PasswordHistory>().Add(passwordHistoryEntry);
        await db.SaveChangesAsync(cancellationToken);

        // Clean up old password history entries
        await CleanupOldPasswordHistoryAsync(user.Id, cancellationToken);
    }

    public async Task CleanupOldPasswordHistoryAsync(string userId, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(userId);

        int passwordHistoryCount = _passwordPolicyOptions.PasswordHistoryCount;
        if (passwordHistoryCount <= 0)
        {
            return; // Password history disabled
        }

        // Get all password history entries for the user, ordered by most recent
        List<PasswordHistory> allPasswordHistories = await db.Set<PasswordHistory>()
            .Where(ph => ph.UserId == userId)
            .OrderByDescending(ph => ph.CreatedAt)
            .ToListAsync(cancellationToken);

        // Keep only the configured number of passwords
        if (allPasswordHistories.Count > passwordHistoryCount)
        {
            List<PasswordHistory> oldPasswordHistories = allPasswordHistories
                .Skip(passwordHistoryCount)
                .ToList();

            db.Set<PasswordHistory>().RemoveRange(oldPasswordHistories);
            await db.SaveChangesAsync(cancellationToken);
        }
    }
}
