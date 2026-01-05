using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.BranchTargets.GetBranchTargets;

public sealed record GetBranchTargetsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<BranchTargetsPagedResponse>;

public sealed record BranchTargetsPagedResponse(List<BranchTargetSummaryDto> Items, int TotalCount, int Page, int PageSize);
