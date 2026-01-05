using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.LoanOfficerTargets.GetLoanOfficerTargets;

public sealed record GetLoanOfficerTargetsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<LoanOfficerTargetsPagedResponse>;

public sealed record LoanOfficerTargetsPagedResponse(List<LoanOfficerTargetSummaryDto> Items, int TotalCount, int Page, int PageSize);
