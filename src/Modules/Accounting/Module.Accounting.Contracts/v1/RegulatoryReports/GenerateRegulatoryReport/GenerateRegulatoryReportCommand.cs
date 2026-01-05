using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.RegulatoryReports.GenerateRegulatoryReport;

public sealed record GenerateRegulatoryReportCommand(Guid Id) : ICommand;
