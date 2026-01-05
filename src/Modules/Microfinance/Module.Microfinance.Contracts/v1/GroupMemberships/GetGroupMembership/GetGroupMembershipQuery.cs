using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.GroupMemberships.GetGroupMembership;

public sealed record GetGroupMembershipQuery(Guid Id) : IQuery<GroupMembershipDto>;
