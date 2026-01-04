namespace FSH.Module.Microfinance.Contracts.v1.Loans;

public record GetLoanQuery(Guid Id);
public record GetLoansQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive);
public record LoansPagedResponse(List<LoanSummaryDto> Items, int TotalCount, int Page, int PageSize);
