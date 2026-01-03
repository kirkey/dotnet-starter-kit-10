namespace FSH.Modules.Microfinance.Contracts.v1.ReportGenerations;

public record GetReportGenerationQuery(Guid Id);
public record GetReportGenerationsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive);
public record ReportGenerationsPagedResponse(List<ReportGenerationSummaryDto> Items, int TotalCount, int Page, int PageSize);
