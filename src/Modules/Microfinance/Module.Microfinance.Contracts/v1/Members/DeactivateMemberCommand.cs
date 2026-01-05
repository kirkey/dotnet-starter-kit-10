using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.Members;

/// <summary>
/// Command to deactivate a member.
/// </summary>
public record DeactivateMemberCommand(Guid Id) : ICommand<Unit>;
