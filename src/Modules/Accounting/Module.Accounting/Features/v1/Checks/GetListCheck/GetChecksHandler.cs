using FSH.Module.Accounting.Contracts.v1.Checks;
using FSH.Module.Accounting.Contracts.v1.Checks.GetListCheck;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.Checks.GetChecks;

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
