using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.CreditBureauReports.GetCreditBureauReports;

public sealed record GetCreditBureauReportsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<CreditBureauReportsPagedResponse>;
