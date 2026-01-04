using Mediator;

namespace FSH.Module.Identity.Contracts.v1.Users.ToggleUserStatus;

public class ToggleUserStatusCommand : ICommand<Unit>
{
    public bool ActivateUser { get; set; }
    public string? UserId { get; set; }
}
