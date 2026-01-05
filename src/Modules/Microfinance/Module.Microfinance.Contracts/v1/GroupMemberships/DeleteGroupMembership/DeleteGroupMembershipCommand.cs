using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.GroupMemberships.DeleteGroupMembership;

public sealed record DeleteGroupMembershipCommand(Guid Id) : ICommand;
