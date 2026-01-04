using FSH.Module.Accounting.Contracts.v1.Banks;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.Banks.GetBanks;

/// <summary>
/// Query to retrieve a paginated list of banks with optional filtering by multiple criteria.
/// </summary>
/// <param name="Page">Page number for pagination (1-based, default=1)</param>
/// <param name="PageSize">Number of items per page (default=10)</param>
/// <param name="SearchTerm">Optional filter by BankName (contains search)</param>
/// <param name="IsActive">Optional filter by active status (null = all)</param>
/// <param name="CurrencyCode">Optional filter by currency code (e.g., "USD", "EUR")</param>
/// <param name="IsDefault">Optional filter by default bank flag (null = all)</param>
public record GetBanksQuery(
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    bool? IsActive = null,
    string? CurrencyCode = null,
    bool? IsDefault = null) : IQuery<BanksPagedResponse>;

/// <summary>
/// Response object for paginated bank list with summary data.
/// </summary>
/// <param name="Items">List of BankSummaryDto with 5 returned fields</param>
/// <param name="TotalCount">Total count of banks matching filters (excluding pagination)</param>
/// <param name="Page">Requested page number</param>
/// <param name="PageSize">Items per page</param>
public record BanksPagedResponse(
    List<BankSummaryDto> Items,
    int TotalCount,
    int Page,
    int PageSize);

/// <summary>
/// Handler for retrieving a paginated, filtered list of bank accounts.
/// </summary>
/// <remarks>
/// Responsibility: Query banks with multiple filters, pagination, and summary projection.
/// 
/// Execution Flow:
/// 1. Build queryable from Banks DbSet
/// 2. Apply SearchTerm filter to BankName using Contains (case-insensitive) if provided
/// 3. Apply IsActive filter using Where(x => x.IsActive == value) if provided
/// 4. Apply CurrencyCode filter using Where(x => x.CurrencyCode == value) if provided
/// 5. Apply IsDefault filter using Where(x => x.IsDefault == value) if provided
/// 6. Get total count before pagination using CountAsync
/// 7. Sort by BankName in ascending order (alphabetical)
/// 8. Apply pagination using Skip((page-1)*pageSize).Take(pageSize)
/// 9. Project to BankSummaryDto with 5 fields: Id, BankName, CurrencyCode, CurrentBalance, IsDefault
/// 10. Execute query and return BanksPagedResponse
/// 
/// Filtering Logic:
/// - SearchTerm: Case-insensitive substring match on BankName field
/// - IsActive: Exact match on IsActive boolean field (null = no filter)
/// - CurrencyCode: Exact match on CurrencyCode field (null = no filter)
/// - IsDefault: Exact match on IsDefault boolean field (null = no filter)
/// 
/// Sorting: BankName ASC (alphabetical order for easy selection)
/// 
/// Pagination: Standard skip-take pattern (page-1)*pageSize
/// 
/// Returned Fields (BankSummaryDto): Id, BankName, CurrencyCode, CurrentBalance, IsDefault
/// 
/// Permissions: Requires authenticated user
/// 
/// Exceptions: None; returns empty list if no matches found
/// </remarks>
public class GetBanksHandler(AccountingDbContext context) 
    : IQueryHandler<GetBanksQuery, BanksPagedResponse>
{
    public async ValueTask<BanksPagedResponse> Handle(GetBanksQuery query, CancellationToken ct)
    {
        var queryable = context.Banks.AsQueryable();
        
        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            queryable = queryable.Where(x => x.BankName.Contains(query.SearchTerm));
        }
        
        if (query.IsActive.HasValue)
        {
            queryable = queryable.Where(x => x.IsActive == query.IsActive.Value);
        }
        
        if (!string.IsNullOrWhiteSpace(query.CurrencyCode))
        {
            queryable = queryable.Where(x => x.CurrencyCode == query.CurrencyCode);
        }
        
        if (query.IsDefault.HasValue)
        {
            queryable = queryable.Where(x => x.IsDefault == query.IsDefault.Value);
        }
        
        var totalCount = await queryable.CountAsync(ct);
        
        var items = await queryable
            .OrderByDescending(x => x.BankName)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(x => new BankSummaryDto(
                x.Id,
                x.BankName,
                x.CurrencyCode,
                x.CurrentBalance,
                x.IsDefault,
                x.IsActive))
            .ToListAsync(ct);
        
        return new BanksPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
