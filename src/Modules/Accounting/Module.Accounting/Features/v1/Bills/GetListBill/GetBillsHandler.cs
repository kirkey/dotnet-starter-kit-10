using FSH.Module.Accounting.Contracts.v1.Bills;
using FSH.Module.Accounting.Contracts.v1.Bills.GetListBill;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.Bills.GetBills;

public class GetBillsHandler(AccountingDbContext context) 
    : IQueryHandler<GetBillsQuery, BillsPagedResponse>
{
    public async ValueTask<BillsPagedResponse> Handle(GetBillsQuery query, CancellationToken ct)
    {
        var queryable = context.Bills.AsQueryable();
        
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
            .Select(x => new BillSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new BillsPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
