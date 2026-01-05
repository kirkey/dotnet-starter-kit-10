using FSH.Module.Accounting.Contracts.v1.GeneralLedger;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.GeneralLedger.GetListGeneralLedger;

/// <summary>
/// Query to retrieve a paginated list of General Ledger accounts with optional name search and active filter.
/// </summary>
/// <param name="Page">Page number for pagination (1-based)</param>
/// <param name="PageSize">Items per page</param>
/// <param name="SearchTerm">Optional substring search on GL Name</param>
/// <param name="IsActive">Optional filter by active status</param>
/// <summary>
/// Handler for listing General Ledger accounts with basic filtering and pagination.
/// </summary>
/// <remarks>
/// Responsibility: Apply SearchTerm and IsActive filters, order by CreatedOnUtc DESC, and return paged summaries.
/// 
/// Returned Fields (GeneralLedgerSummaryDto): Id, Name, IsActive
/// 
/// Permissions: Requires GeneralLedger.Search/View
/// 
/// Exceptions: None; returns empty list if no matches
/// </remarks>
public class GetGeneralLedgerListHandler(AccountingDbContext context) 
    : IQueryHandler<FSH.Module.Accounting.Contracts.v1.GeneralLedger.GetListGeneralLedger.GetListGeneralLedgerQuery, FSH.Module.Accounting.Contracts.v1.GeneralLedger.GetListGeneralLedger.GeneralLedgerPagedResponse>
{
    public async ValueTask<FSH.Module.Accounting.Contracts.v1.GeneralLedger.GetListGeneralLedger.GeneralLedgerPagedResponse> Handle(FSH.Module.Accounting.Contracts.v1.GeneralLedger.GetListGeneralLedger.GetListGeneralLedgerQuery query, CancellationToken ct)
    {
        var queryable = context.GeneralLedger.AsQueryable();
        
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
            .Select(x => new GeneralLedgerSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new FSH.Module.Accounting.Contracts.v1.GeneralLedger.GetListGeneralLedger.GeneralLedgerPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
