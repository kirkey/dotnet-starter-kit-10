namespace FSH.Module.Microfinance.Contracts.v1.LoanGuarantors;

public record GetLoanGuarantorQuery(Guid Id);
public record GetLoanGuarantorsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive);
public record LoanGuarantorsPagedResponse(List<LoanGuarantorSummaryDto> Items, int TotalCount, int Page, int PageSize);
