namespace FSH.Module.Microfinance.Contracts.v1.ReportDefinitions;

public record GetReportDefinitionQuery(Guid Id);
public record GetReportDefinitionsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive);
public record ReportDefinitionsPagedResponse(List<ReportDefinitionSummaryDto> Items, int TotalCount, int Page, int PageSize);
