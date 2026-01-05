using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.GroupMemberships.GetGroupMemberships;

public sealed record GetGroupMembershipsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<GroupMembershipsPagedResponse>;

public sealed record GroupMembershipsPagedResponse(List<GroupMembershipSummaryDto> Items, int TotalCount, int Page, int PageSize);
