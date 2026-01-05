namespace FSH.Module.Microfinance.Contracts.v1.MemberGroups;

public record MemberGroupsPagedResponse(List<MemberGroupSummaryDto> Items, int TotalCount, int Page, int PageSize);
