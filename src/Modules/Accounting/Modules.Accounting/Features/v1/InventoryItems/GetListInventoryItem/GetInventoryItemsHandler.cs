using FSH.Modules.Accounting.Contracts.v1.InventoryItems;
using FSH.Modules.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Accounting.Features.v1.InventoryItems.GetInventoryItems;

public record GetInventoryItemsQuery(
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    bool? IsActive = null) : IQuery<InventoryItemsPagedResponse>;

public record InventoryItemsPagedResponse(
    List<InventoryItemSummaryDto> Items,
    int TotalCount,
    int Page,
    int PageSize);

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
