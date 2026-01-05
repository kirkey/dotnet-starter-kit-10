using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.GroupMemberships.UpdateGroupMembership;

public sealed record UpdateGroupMembershipCommand(Guid Id, string Name) : ICommand<Guid>;
