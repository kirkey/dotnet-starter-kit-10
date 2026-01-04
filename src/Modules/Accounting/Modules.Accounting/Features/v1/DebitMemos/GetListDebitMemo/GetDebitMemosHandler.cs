using FSH.Modules.Accounting.Contracts.v1.DebitMemos;
using FSH.Modules.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Accounting.Features.v1.DebitMemos.GetDebitMemos;

public record GetDebitMemosQuery(
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    bool? IsActive = null) : IQuery<DebitMemosPagedResponse>;

public record DebitMemosPagedResponse(
    List<DebitMemoSummaryDto> Items,
    int TotalCount,
    int Page,
    int PageSize);

public class GetDebitMemosHandler(AccountingDbContext context) 
    : IQueryHandler<GetDebitMemosQuery, DebitMemosPagedResponse>
{
    public async ValueTask<DebitMemosPagedResponse> Handle(GetDebitMemosQuery query, CancellationToken ct)
    {
        var queryable = context.DebitMemos.AsQueryable();
        
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
            .Select(x => new DebitMemoSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new DebitMemosPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
