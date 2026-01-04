using FSH.Module.Accounting.Contracts.v1.Payments;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.Payments.GetPayments;

/// <summary>
/// Get Payments list query DTO.
/// 
/// **Purpose:**
/// Encapsulates the request to retrieve a paginated list of Payments with filtering.
/// 
/// **Parameters:**
/// - Page: Page number for pagination (default: 1)
/// - PageSize: Number of items per page (default: 10)
/// - SearchTerm: Text search across Payment Name
/// - IsActive: Filter by active status (null = all)
/// 
/// **Multi-Tenancy:**
/// Tenant context is automatically applied via query filters.
/// </summary>
public record GetPaymentsQuery(
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    bool? IsActive = null) : IQuery<PaymentsPagedResponse>;

public record PaymentsPagedResponse(
    List<PaymentSummaryDto> Items,
    int TotalCount,
    int Page,
    int PageSize);

/// <summary>
/// Handler for retrieving paginated list of Payments.
/// 
/// **Responsibility:**
/// Queries the database for Payments with filtering, sorting, and pagination.
/// Returns a summary DTO for each payment.
/// 
/// **Execution Flow:**
/// 1. Build base queryable from DbSet
/// 2. Apply SearchTerm filter (Name contains)
/// 3. Apply IsActive filter (if specified)
/// 4. Count total matching records
/// 5. Sort by CreatedOnUtc descending (most recent first)
/// 6. Skip and take for pagination
/// 7. Project to summary DTOs
/// 8. Return paged response
/// 
/// **Filtering Logic:**
/// - SearchTerm: Case-sensitive substring match on Name
/// - IsActive: Exact match on boolean flag
/// 
/// **Sorting:**
/// Primary: CreatedOnUtc (descending - most recent first)
/// 
/// **Permissions:**
/// Requires: Accounting.Payment.View
/// </summary>
public class GetPaymentsHandler(AccountingDbContext context) 
    : IQueryHandler<GetPaymentsQuery, PaymentsPagedResponse>
{
    /// <summary>
    /// Handles the GetPaymentsQuery to retrieve a paginated list.
    /// </summary>
    /// <param name="query">The query containing pagination and filter parameters</param>
    /// <param name="ct">Cancellation token for the operation</param>
    /// <returns>Paged response with payment summaries and total count</returns>
    public async ValueTask<PaymentsPagedResponse> Handle(GetPaymentsQuery query, CancellationToken ct)
    {
        var queryable = context.Payments.AsQueryable();
        
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
            .Select(x => new PaymentSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new PaymentsPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
