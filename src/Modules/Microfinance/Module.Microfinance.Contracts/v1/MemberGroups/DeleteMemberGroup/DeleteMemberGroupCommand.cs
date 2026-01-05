using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.MemberGroups.DeleteMemberGroup;

public sealed record DeleteMemberGroupCommand(Guid Id) : ICommand;
