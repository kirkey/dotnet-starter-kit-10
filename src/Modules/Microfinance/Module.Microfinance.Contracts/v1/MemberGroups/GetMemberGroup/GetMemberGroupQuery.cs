using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.MemberGroups.GetMemberGroup;

public sealed record GetMemberGroupQuery(Guid Id) : IQuery<MemberGroupDto>;
