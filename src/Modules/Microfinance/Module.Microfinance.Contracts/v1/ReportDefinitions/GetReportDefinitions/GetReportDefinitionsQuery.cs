using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.ReportDefinitions.GetReportDefinitions;

public sealed record GetReportDefinitionsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<ReportDefinitionsPagedResponse>;
