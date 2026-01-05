using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.RegulatoryReports.SubmitRegulatoryReport;

public sealed record SubmitRegulatoryReportCommand(Guid Id) : ICommand;
