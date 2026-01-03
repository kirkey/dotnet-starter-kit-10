namespace FSH.Modules.Microfinance.Contracts.v1.CustomerCases;

public record GetCustomerCaseQuery(Guid Id);
public record GetCustomerCasesQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive);
public record CustomerCasesPagedResponse(List<CustomerCaseSummaryDto> Items, int TotalCount, int Page, int PageSize);
