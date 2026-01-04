using FSH.Modules.Accounting.Contracts.v1.Bills;
using FSH.Modules.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Accounting.Features.v1.Bills.GetBills;

public record GetBillsQuery(
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    bool? IsActive = null) : IQuery<BillsPagedResponse>;

public record BillsPagedResponse(
    List<BillSummaryDto> Items,
    int TotalCount,
    int Page,
    int PageSize);

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
