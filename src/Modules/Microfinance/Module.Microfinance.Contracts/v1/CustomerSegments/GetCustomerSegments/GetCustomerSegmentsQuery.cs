using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.CustomerSegments.GetCustomerSegments;

public sealed record GetCustomerSegmentsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<CustomerSegmentsPagedResponse>;
