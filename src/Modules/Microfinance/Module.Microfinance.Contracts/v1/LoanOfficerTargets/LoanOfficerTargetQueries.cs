namespace FSH.Module.Microfinance.Contracts.v1.LoanOfficerTargets;

public record GetLoanOfficerTargetQuery(Guid Id);
public record GetLoanOfficerTargetsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive);
public record LoanOfficerTargetsPagedResponse(List<LoanOfficerTargetSummaryDto> Items, int TotalCount, int Page, int PageSize);
