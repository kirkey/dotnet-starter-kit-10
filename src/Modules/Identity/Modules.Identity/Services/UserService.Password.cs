using FSH.Framework.Core.Exceptions;
using FSH.Framework.Mailing;
using FSH.Modules.Identity.Features.v1.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Collections.ObjectModel;
using System.Text;

namespace FSH.Modules.Identity.Services;

internal sealed partial class UserService
{
    public async Task ForgotPasswordAsync(string email, string origin, CancellationToken cancellationToken)
    {
        EnsureValidTenant();

        FshUser? user = await userManager.FindByEmailAsync(email);
        if (user == null)
        {
            throw new NotFoundException("user not found");
        }

        if (string.IsNullOrWhiteSpace(user.Email))
        {
            throw new InvalidOperationException("user email cannot be null or empty");
        }

        string token = await userManager.GeneratePasswordResetTokenAsync(user);
        token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

        string resetPasswordUri = $"{origin}/reset-password?token={token}&email={email}";
        MailRequest mailRequest = new(
            new Collection<string> { user.Email },
            "Reset Password",
            $"Please reset your password using the following link: {resetPasswordUri}");

        jobService.Enqueue(() => mailService.SendAsync(mailRequest, CancellationToken.None));
    }

    public async Task ResetPasswordAsync(string email, string password, string token, CancellationToken cancellationToken)
    {
        EnsureValidTenant();

        FshUser? user = await userManager.FindByEmailAsync(email);
        if (user == null)
        {
            throw new NotFoundException("user not found");
        }

        token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
        IdentityResult result = await userManager.ResetPasswordAsync(user, token, password);

        if (!result.Succeeded)
        {
            List<string> errors = result.Errors.Select(e => e.Description).ToList();
            throw new CustomException("error resetting password", errors);
        }
    }

    public async Task ChangePasswordAsync(string password, string newPassword, string confirmNewPassword, string userId)
    {
        FshUser? user = await userManager.FindByIdAsync(userId);

        _ = user ?? throw new NotFoundException("user not found");

        IdentityResult result = await userManager.ChangePasswordAsync(user, password, newPassword);

        if (!result.Succeeded)
        {
            List<string> errors = result.Errors.Select(e => e.Description).ToList();
            throw new CustomException("failed to change password", errors);
        }

        // Save the old password hash to history after successful password change
        // Reload user to get the new password hash
        user = await userManager.FindByIdAsync(userId);
        if (user is not null)
        {
            // Update password expiry date
            _passwordExpiryService.UpdateLastPasswordChangeDate(user);

            // Save to history
            await _passwordHistoryService.SavePasswordHistoryAsync(user);

            // Update user with new password change date
            await userManager.UpdateAsync(user);
        }
    }
}