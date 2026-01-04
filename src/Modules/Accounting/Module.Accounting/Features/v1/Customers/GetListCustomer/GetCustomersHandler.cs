using FSH.Module.Accounting.Contracts.v1.Customers;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.Customers.GetCustomers;

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
/// Handler for retrieving a paginated, filtered list of customers.
/// </summary>
/// <remarks>
/// Responsibility: Query customers with multiple filters, pagination, and summary projection.
/// 
/// Execution Flow:
/// 1. Build queryable from Customers DbSet
/// 2. Apply SearchTerm filter to Name using Contains (case-insensitive) if provided
/// 3. Apply IsActive filter using Where(x => x.IsActive == value) if provided
/// 4. Get total count before pagination using CountAsync
/// 5. Sort by CreatedOnUtc in descending order (most recently created first)
/// 6. Apply pagination using Skip((page-1)*pageSize).Take(pageSize)
/// 7. Project to CustomerSummaryDto with 3 fields: Id, Name, IsActive
/// 8. Execute query and return CustomersPagedResponse
/// 
/// Filtering Logic:
/// - SearchTerm: Case-insensitive substring match on Name field
/// - IsActive: Exact match on IsActive boolean field (null = no filter)
/// 
/// Sorting: CreatedOnUtc DESC (most recently created customers first)
/// 
/// Pagination: Standard skip-take pattern (page-1)*pageSize
/// 
/// Returned Fields (CustomerSummaryDto): Id, Name, IsActive
/// 
/// Permissions: Requires authenticated user
/// 
/// Exceptions: None; returns empty list if no matches found
/// </remarks>
public class GetCustomersHandler(AccountingDbContext context) 
    : IQueryHandler<GetCustomersQuery, CustomersPagedResponse>
{
    public async ValueTask<CustomersPagedResponse> Handle(GetCustomersQuery query, CancellationToken ct)
    {
        var queryable = context.Customers.AsQueryable();
        
        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            queryable = queryable.Where(x => x.Name.Contains(query.SearchTerm));
        }
        
        if (query.IsActive.HasValue)
        {
            queryable = queryable.Where(x => x.IsActive == query.IsActive.Value);
        }
        
        var totalCount = await queryable.CountAsync(ct);
        
        var items = await queryable
            .OrderByDescending(x => x.CreatedOnUtc)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(x => new CustomerSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new CustomersPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
