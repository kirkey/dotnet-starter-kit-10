using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.RegulatoryReports.ExportRegulatoryReport;

public sealed record ExportRegulatoryReportCommand(Guid Id) : ICommand;
