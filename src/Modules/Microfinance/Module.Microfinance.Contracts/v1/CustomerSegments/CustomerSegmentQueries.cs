namespace FSH.Module.Microfinance.Contracts.v1.CustomerSegments;

public record GetCustomerSegmentQuery(Guid Id);
public record GetCustomerSegmentsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive);
public record CustomerSegmentsPagedResponse(List<CustomerSegmentSummaryDto> Items, int TotalCount, int Page, int PageSize);
