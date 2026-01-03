namespace FSH.Modules.Microfinance.Contracts.v1.CreditBureauReports;

public record GetCreditBureauReportQuery(Guid Id);
public record GetCreditBureauReportsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive);
public record CreditBureauReportsPagedResponse(List<CreditBureauReportSummaryDto> Items, int TotalCount, int Page, int PageSize);
