using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.RegulatoryReports.GetRegulatoryReport;

public sealed record GetRegulatoryReportQuery(Guid Id) : IQuery<RegulatoryReportDto>;
