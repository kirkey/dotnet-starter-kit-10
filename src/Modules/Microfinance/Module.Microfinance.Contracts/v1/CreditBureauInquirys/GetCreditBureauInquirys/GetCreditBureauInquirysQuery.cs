using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.CreditBureauInquirys.GetCreditBureauInquirys;

public sealed record GetCreditBureauInquirysQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<CreditBureauInquirysPagedResponse>;

public sealed record CreditBureauInquirysPagedResponse(List<CreditBureauInquirySummaryDto> Items, int TotalCount, int Page, int PageSize);
