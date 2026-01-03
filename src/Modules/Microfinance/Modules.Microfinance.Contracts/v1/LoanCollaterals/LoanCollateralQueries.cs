namespace FSH.Modules.Microfinance.Contracts.v1.LoanCollaterals;

public record GetLoanCollateralQuery(Guid Id);
public record GetLoanCollateralsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive);
public record LoanCollateralsPagedResponse(List<LoanCollateralSummaryDto> Items, int TotalCount, int Page, int PageSize);
