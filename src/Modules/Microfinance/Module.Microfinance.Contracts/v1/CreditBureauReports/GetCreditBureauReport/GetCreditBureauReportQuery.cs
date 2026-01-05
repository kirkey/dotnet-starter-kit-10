using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.CreditBureauReports.GetCreditBureauReport;

public sealed record GetCreditBureauReportQuery(Guid Id) : IQuery<CreditBureauReportDto>;
