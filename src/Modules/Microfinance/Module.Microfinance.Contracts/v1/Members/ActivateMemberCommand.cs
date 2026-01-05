using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.Members;

/// <summary>
/// Command to activate a member.
/// </summary>
public record ActivateMemberCommand(Guid Id) : ICommand<Guid>;
