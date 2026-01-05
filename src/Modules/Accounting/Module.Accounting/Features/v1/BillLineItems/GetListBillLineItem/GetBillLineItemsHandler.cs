using FSH.Module.Accounting.Contracts.v1.BillLineItems;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;
using FSH.Module.Accounting.Contracts.v1.BillLineItems.GetListBillLineItem;

namespace FSH.Module.Accounting.Features.v1.BillLineItems.GetBillLineItems;

public class GetBillLineItemsHandler(AccountingDbContext context) 
    : IQueryHandler<GetBillLineItemsQuery, BillLineItemsPagedResponse>
{
    public async ValueTask<BillLineItemsPagedResponse> Handle(GetBillLineItemsQuery query, CancellationToken ct)
    {
        var queryable = context.BillLineItems.AsQueryable();
        
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
            .Select(x => new BillLineItemSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new BillLineItemsPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
