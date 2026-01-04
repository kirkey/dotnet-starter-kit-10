using FSH.Module.Accounting.Contracts.v1.Checks;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.Checks.GetChecks;

/// <summary>
/// Query to retrieve a paginated list of checks with optional filtering by multiple criteria.
/// </summary>
/// <param name="Page">Page number for pagination (1-based, default=1)</param>
/// <param name="PageSize">Number of items per page (default=10)</param>
/// <param name="SearchTerm">Optional filter by CheckNumber, PayeeName, or AccountNumber (contains search)</param>
/// <param name="IsActive">Optional filter by active status (null = all)</param>
/// <param name="Status">Optional filter by check status (e.g., "Draft", "Printed", "Issued", "Cleared", "Voided")</param>
/// <param name="FromDate">Optional filter by check date >= FromDate (inclusive)</param>
/// <param name="ToDate">Optional filter by check date <= ToDate (inclusive)</param>
public record GetChecksQuery(
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    bool? IsActive = null,
    string? Status = null,
    DateTime? FromDate = null,
    DateTime? ToDate = null) : IQuery<ChecksPagedResponse>;

/// <summary>
/// Response object for paginated check list with summary data.
/// </summary>
/// <param name="Items">List of CheckSummaryDto with summary fields</param>
/// <param name="TotalCount">Total count of checks matching filters (excluding pagination)</param>
/// <param name="Page">Requested page number</param>
/// <param name="PageSize">Items per page</param>
public record ChecksPagedResponse(
    List<CheckSummaryDto> Items,
    int TotalCount,
    int Page,
    int PageSize);

/// <summary>
/// Handler for retrieving a paginated, filtered list of checks with complex date range filtering.
/// </summary>
/// <remarks>
/// Responsibility: Query checks with multiple filters, date range, pagination, and summary projection.
/// 
/// Execution Flow:
/// 1. Build queryable from Checks DbSet
/// 2. Apply SearchTerm filter (multi-field): CheckNumber OR PayeeName OR AccountNumber (contains search) if provided
/// 3. Apply IsActive filter using Where(x => x.IsActive == value) if provided
/// 4. Apply Status filter using Where(x => x.Status == value) if provided
/// 5. Apply FromDate filter using Where(x => x.CheckDate >= FromDate) if provided (inclusive)
/// 6. Apply ToDate filter using Where(x => x.CheckDate <= ToDate) if provided (inclusive)
/// 7. Get total count before pagination using CountAsync
/// 8. Sort by CheckDate in ascending order (oldest first)
/// 9. Apply pagination using Skip((page-1)*pageSize).Take(pageSize)
/// 10. Project to CheckSummaryDto with summary fields
/// 11. Execute query and return ChecksPagedResponse
/// 
/// Filtering Logic:
/// - SearchTerm: Case-insensitive substring match on CheckNumber, PayeeName, or AccountNumber (OR combined)
/// - IsActive: Exact match on IsActive boolean field (null = no filter)
/// - Status: Exact match on Status field (null = no filter, useful for workflow filtering)
/// - FromDate: Inclusive >= comparison on CheckDate (date range start)
/// - ToDate: Inclusive <= comparison on CheckDate (date range end)
/// 
/// Sorting: CheckDate ASC (oldest checks first for sequential processing)
/// 
/// Pagination: Standard skip-take pattern (page-1)*pageSize
/// 
/// Returned Fields (CheckSummaryDto): Id, CheckNumber, CheckDate, PayeeName, Amount, Status
/// 
/// Use Cases:
/// - Outstanding checks report (Status != 'Cleared')
/// - Payee history (SearchTerm = PayeeName + date range)
/// - Bank reconciliation (FromDate/ToDate filtered list by clearing status)
/// - Check register (all checks ordered by date)
/// 
/// Permissions: Requires authenticated user
/// 
/// Exceptions: None; returns empty list if no matches found
/// </remarks>
public class GetChecksHandler(AccountingDbContext context) 
    : IQueryHandler<GetChecksQuery, ChecksPagedResponse>
{
    public async ValueTask<ChecksPagedResponse> Handle(GetChecksQuery query, CancellationToken ct)
    {
        var queryable = context.Checks.AsQueryable();
        
        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            queryable = queryable.Where(x => 
                x.CheckNumber.Contains(query.SearchTerm) ||
                x.PayeeName.Contains(query.SearchTerm) ||
                x.AccountNumber.Contains(query.SearchTerm));
        }
        
        if (query.IsActive.HasValue)
        {
            queryable = queryable.Where(x => x.IsActive == query.IsActive.Value);
        }
        
        if (!string.IsNullOrWhiteSpace(query.Status))
        {
            queryable = queryable.Where(x => x.Status == query.Status);
        }
        
        if (query.FromDate.HasValue)
        {
            queryable = queryable.Where(x => x.CheckDate >= query.FromDate.Value);
        }
        
        if (query.ToDate.HasValue)
        {
            queryable = queryable.Where(x => x.CheckDate <= query.ToDate.Value);
        }
        
        var totalCount = await queryable.CountAsync(ct);
        
        var items = await queryable
            .OrderByDescending(x => x.CheckDate)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(x => new CheckSummaryDto(
                x.Id,
                x.CheckNumber,
                x.CheckDate,
                x.PayeeName,
                x.Amount,
                x.Status,
                x.IsActive))
            .ToListAsync(ct);
        
        return new ChecksPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
