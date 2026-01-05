namespace FSH.Module.Microfinance.Contracts.v1.ReportDefinitions;


public record ReportDefinitionsPagedResponse(List<ReportDefinitionSummaryDto> Items, int TotalCount, int Page, int PageSize);
