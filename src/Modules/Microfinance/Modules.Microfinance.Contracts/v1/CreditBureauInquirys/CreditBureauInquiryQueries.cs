namespace FSH.Modules.Microfinance.Contracts.v1.CreditBureauInquirys;

public record GetCreditBureauInquiryQuery(Guid Id);
public record GetCreditBureauInquirysQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive);
public record CreditBureauInquirysPagedResponse(List<CreditBureauInquirySummaryDto> Items, int TotalCount, int Page, int PageSize);
