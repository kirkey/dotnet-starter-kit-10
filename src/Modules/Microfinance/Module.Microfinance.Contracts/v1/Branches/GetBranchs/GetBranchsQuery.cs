using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.Branches.GetBranchs;

public sealed record GetBranchsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<BranchsPagedResponse>;

public sealed record BranchsPagedResponse(List<BranchSummaryDto> Items, int TotalCount, int Page, int PageSize);
