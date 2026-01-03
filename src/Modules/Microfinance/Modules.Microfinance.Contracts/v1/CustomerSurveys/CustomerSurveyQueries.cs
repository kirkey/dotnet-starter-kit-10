namespace FSH.Modules.Microfinance.Contracts.v1.CustomerSurveys;

public record GetCustomerSurveyQuery(Guid Id);
public record GetCustomerSurveysQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive);
public record CustomerSurveysPagedResponse(List<CustomerSurveySummaryDto> Items, int TotalCount, int Page, int PageSize);
