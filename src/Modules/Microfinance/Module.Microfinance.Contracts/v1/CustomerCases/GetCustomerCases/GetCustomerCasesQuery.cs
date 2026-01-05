using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.CustomerCases.GetCustomerCases;

public sealed record GetCustomerCasesQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<CustomerCasesPagedResponse>;

public sealed record CustomerCasesPagedResponse(List<CustomerCaseSummaryDto> Items, int TotalCount, int Page, int PageSize);
