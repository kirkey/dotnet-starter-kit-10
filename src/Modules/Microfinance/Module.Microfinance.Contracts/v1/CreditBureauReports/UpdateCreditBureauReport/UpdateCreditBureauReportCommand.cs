using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.CreditBureauReports.UpdateCreditBureauReport;

public sealed record UpdateCreditBureauReportCommand(Guid Id, string Name) : ICommand<Guid>;
