using FSH.Module.Accounting.Contracts.v1.Customers;
using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.Customers.GetCustomers;

/// <summary>
/// Query to retrieve a paginated list of customers with optional filtering.
/// </summary>
/// <param name="Page">Page number for pagination (1-based, default=1)</param>
/// <param name="PageSize">Number of items per page (default=10)</param>
/// <param name="SearchTerm">Optional filter by customer Name (contains search)</param>
/// <param name="IsActive">Optional filter by active status (null = all, true = active only, false = inactive only)</param>
public record GetCustomersQuery(
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    bool? IsActive = null) : IQuery<CustomersPagedResponse>;

/// <summary>
/// Response object for paginated customer list with summary data.
/// </summary>
/// <param name="Items">List of CustomerSummaryDto with 3 returned fields: Id, Name, IsActive</param>
/// <param name="TotalCount">Total count of customers matching filters (excluding pagination)</param>
/// <param name="Page">Requested page number</param>
/// <param name="PageSize">Items per page</param>
public record CustomersPagedResponse(
    List<CustomerSummaryDto> Items,
    int TotalCount,
    int Page,
    int PageSize);

/// <summary>
/// Summary projection of a customer for list views.
/// </summary>
/// <param name="Id">Customer unique identifier</param>
/// <param name="Name">Customer business or individual name</param>
/// <param name="IsActive">Whether customer is currently active</param>
public record CustomerSummaryDto(Guid Id, string Name, bool IsActive);