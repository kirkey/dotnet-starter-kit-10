using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.MemberGroups.CreateMemberGroup;

public sealed record CreateMemberGroupCommand(string Name) : ICommand<Guid>;
