using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.CreditBureauReports.CreateCreditBureauReport;

public sealed record CreateCreditBureauReportCommand(string Name) : ICommand<Guid>;
