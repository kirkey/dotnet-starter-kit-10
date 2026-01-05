using FSH.Module.Accounting.Contracts.v1.RetainedEarnings.GetListRetainedEarnings;
using FSH.Module.Accounting.Contracts.v1.RetainedEarnings;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.RetainedEarnings.GetRetainedEarnings;



public class GetRetainedEarningsHandler(AccountingDbContext context) 
    : IQueryHandler<GetRetainedEarningsQuery, RetainedEarningsPagedResponse>
{
    public async ValueTask<RetainedEarningsPagedResponse> Handle(GetRetainedEarningsQuery query, CancellationToken ct)
    {
        var queryable = context.RetainedEarnings.AsQueryable();
        
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
            .Select(x => new RetainedEarningsSummaryDto(
                x.Id,
                x.Name,
                x.FiscalYear,
                x.ClosingBalance,
                x.IsClosed,
                x.IsActive))
            .ToListAsync(ct);
        
        return new RetainedEarningsPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
