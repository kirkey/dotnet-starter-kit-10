namespace FSH.Module.Microfinance.Contracts.v1.LoanRestructures;

public record GetLoanRestructureQuery(Guid Id);
public record GetLoanRestructuresQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive);
public record LoanRestructuresPagedResponse(List<LoanRestructureSummaryDto> Items, int TotalCount, int Page, int PageSize);
