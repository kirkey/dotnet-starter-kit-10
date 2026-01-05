using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.CreditBureauReports.DeleteCreditBureauReport;

public sealed record DeleteCreditBureauReportCommand(Guid Id) : ICommand;
