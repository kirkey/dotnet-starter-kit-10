using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.GroupMemberships.CreateGroupMembership;

public sealed record CreateGroupMembershipCommand(string Name) : ICommand<Guid>;
