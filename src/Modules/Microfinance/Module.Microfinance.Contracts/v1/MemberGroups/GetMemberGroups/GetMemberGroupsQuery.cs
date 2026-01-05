using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.MemberGroups.GetMemberGroups;

public sealed record GetMemberGroupsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<MemberGroupsPagedResponse>;
