namespace FSH.Modules.Microfinance.Contracts.v1.Branches;

public record GetBranchQuery(Guid Id);
public record GetBranchsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive);
public record BranchsPagedResponse(List<BranchSummaryDto> Items, int TotalCount, int Page, int PageSize);
