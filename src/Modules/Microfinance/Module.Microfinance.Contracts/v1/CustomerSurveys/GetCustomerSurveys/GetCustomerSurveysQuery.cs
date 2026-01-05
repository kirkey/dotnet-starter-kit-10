using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.CustomerSurveys.GetCustomerSurveys;

public sealed record GetCustomerSurveysQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<CustomerSurveysPagedResponse>;

public sealed record CustomerSurveysPagedResponse(List<CustomerSurveySummaryDto> Items, int TotalCount, int Page, int PageSize);
