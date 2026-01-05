using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.CreditBureauInquirys.GetCreditBureauInquirys;

public sealed record GetCreditBureauInquirysQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<CreditBureauInquirysPagedResponse>;
