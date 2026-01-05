using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.LoanCollaterals.GetLoanCollaterals;

public sealed record GetLoanCollateralsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<LoanCollateralsPagedResponse>;

public sealed record LoanCollateralsPagedResponse(List<LoanCollateralSummaryDto> Items, int TotalCount, int Page, int PageSize);
