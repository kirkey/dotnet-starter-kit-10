using FSH.Framework.Shared.Identity.Claims;
using FSH.Modules.Identity.Contracts.Services;
using FSH.Modules.Identity.Contracts.v1.Users.ChangePassword;
using Mediator;
using Microsoft.AspNetCore.Http;

namespace FSH.Modules.Identity.Features.v1.Users.ChangePassword;

public sealed class ChangePasswordCommandHandler(IUserService userService, IHttpContextAccessor httpContextAccessor)
    : ICommandHandler<ChangePasswordCommand, string>
{
    public async ValueTask<string> Handle(ChangePasswordCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        string? userId = httpContextAccessor.HttpContext?.User.GetUserId();
        if (string.IsNullOrEmpty(userId))
        {
            throw new InvalidOperationException("User is not authenticated.");
        }

        await userService.ChangePasswordAsync(command.Password, command.NewPassword, command.ConfirmNewPassword, userId).ConfigureAwait(false);

        return "password reset email sent";
    }
}
