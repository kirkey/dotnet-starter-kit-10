namespace FSH.Modules.Microfinance.Contracts.v1.BranchTargets;

public record GetBranchTargetQuery(Guid Id);
public record GetBranchTargetsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive);
public record BranchTargetsPagedResponse(List<BranchTargetSummaryDto> Items, int TotalCount, int Page, int PageSize);
