using FSH.Module.Accounting.Contracts.v1.InventoryItems;
using FSH.Module.Accounting.Contracts.v1.InventoryItems.GetListInventoryItem;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.InventoryItems.GetInventoryItems;

public class GetInventoryItemsHandler(AccountingDbContext context) 
    : IQueryHandler<GetInventoryItemsQuery, InventoryItemsPagedResponse>
{
    public async ValueTask<InventoryItemsPagedResponse> Handle(GetInventoryItemsQuery query, CancellationToken ct)
    {
        var queryable = context.InventoryItems.AsQueryable();
        
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
            .Select(x => new InventoryItemSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new InventoryItemsPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
