using Mediator;
using FSH.Module.Accounting.Contracts.v1.RegulatoryReports;

namespace FSH.Module.Accounting.Contracts.v1.RegulatoryReports.GetListRegulatoryReport;

public sealed record GetRegulatoryReportsQuery(
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    bool? IsActive = null) : IQuery<RegulatoryReportsPagedResponse>;

public sealed record RegulatoryReportsPagedResponse(
    List<RegulatoryReportSummaryDto> Items,
    int TotalCount,
    int Page,
    int PageSize);
