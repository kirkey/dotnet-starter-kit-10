namespace FSH.Modules.Microfinance.Contracts.v1.LoanDisbursementTranches;

public record GetLoanDisbursementTrancheQuery(Guid Id);
public record GetLoanDisbursementTranchesQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive);
public record LoanDisbursementTranchesPagedResponse(List<LoanDisbursementTrancheSummaryDto> Items, int TotalCount, int Page, int PageSize);
