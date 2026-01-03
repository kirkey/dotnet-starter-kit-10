namespace FSH.Modules.Microfinance.Contracts.v1.GroupMemberships;

public record GetGroupMembershipQuery(Guid Id);
public record GetGroupMembershipsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive);
public record GroupMembershipsPagedResponse(List<GroupMembershipSummaryDto> Items, int TotalCount, int Page, int PageSize);
