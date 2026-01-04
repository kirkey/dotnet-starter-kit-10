using FSH.Module.Identity.Contracts.Services;
using FSH.Module.Identity.Contracts.v1.Users.DeleteUser;
using Mediator;

namespace FSH.Module.Identity.Features.v1.Users.DeleteUser;

public sealed class DeleteUserCommandHandler(IUserService userService) : ICommandHandler<DeleteUserCommand, Unit>
{
    public async ValueTask<Unit> Handle(DeleteUserCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        await userService.DeleteAsync(command.Id).ConfigureAwait(false);

        return Unit.Value;
    }
}
