using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.MemberGroups.UpdateMemberGroup;

public sealed record UpdateMemberGroupCommand(Guid Id, string Name) : ICommand<Guid>;
