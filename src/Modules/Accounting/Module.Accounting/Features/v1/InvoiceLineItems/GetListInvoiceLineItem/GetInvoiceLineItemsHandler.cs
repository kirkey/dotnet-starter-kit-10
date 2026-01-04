using FSH.Module.Accounting.Contracts.v1.InvoiceLineItems;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.InvoiceLineItems.GetInvoiceLineItems;

public record GetInvoiceLineItemsQuery(
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    bool? IsActive = null) : IQuery<InvoiceLineItemsPagedResponse>;

public record InvoiceLineItemsPagedResponse(
    List<InvoiceLineItemSummaryDto> Items,
    int TotalCount,
    int Page,
    int PageSize);

public class GetInvoiceLineItemsHandler(AccountingDbContext context) 
    : IQueryHandler<GetInvoiceLineItemsQuery, InvoiceLineItemsPagedResponse>
{
    public async ValueTask<InvoiceLineItemsPagedResponse> Handle(GetInvoiceLineItemsQuery query, CancellationToken ct)
    {
        var queryable = context.InvoiceLineItems.AsQueryable();
        
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
            .Select(x => new InvoiceLineItemSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new InvoiceLineItemsPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
