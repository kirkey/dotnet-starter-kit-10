using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.Members;

/// <summary>
/// Command to delete a member.
/// </summary>
public record DeleteMemberCommand(Guid Id) : ICommand<Guid>;
