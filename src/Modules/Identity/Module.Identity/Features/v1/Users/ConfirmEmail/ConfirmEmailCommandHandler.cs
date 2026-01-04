using FSH.Module.Identity.Contracts.Services;
using FSH.Module.Identity.Contracts.v1.Users.ConfirmEmail;
using Mediator;

namespace FSH.Module.Identity.Features.v1.Users.ConfirmEmail;

public sealed class ConfirmEmailCommandHandler(IUserService userService) : ICommandHandler<ConfirmEmailCommand, string>
{
    public async ValueTask<string> Handle(ConfirmEmailCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        return await userService.ConfirmEmailAsync(command.UserId, command.Code, command.Tenant, cancellationToken)
            .ConfigureAwait(false);
    }
}
