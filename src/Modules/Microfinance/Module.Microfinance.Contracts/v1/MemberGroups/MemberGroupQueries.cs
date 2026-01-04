namespace FSH.Module.Microfinance.Contracts.v1.MemberGroups;

public record GetMemberGroupQuery(Guid Id);
public record GetMemberGroupsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive);
public record MemberGroupsPagedResponse(List<MemberGroupSummaryDto> Items, int TotalCount, int Page, int PageSize);
