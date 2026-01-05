using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.Branches.GetBranchs;

public sealed record GetBranchsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<BranchsPagedResponse>;
