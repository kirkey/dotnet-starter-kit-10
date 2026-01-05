namespace FSH.Module.Microfinance.Contracts.v1.LoanDisbursementTranches;

public record LoanDisbursementTranchesPagedResponse(List<LoanDisbursementTrancheSummaryDto> Items, int TotalCount, int Page, int PageSize);
