using FSH.Modules.Accounting.Contracts.v1.Accruals;
using FSH.Modules.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Accounting.Features.v1.Accruals.GetAccruals;

public record GetAccrualsQuery(
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    bool? IsActive = null) : IQuery<AccrualsPagedResponse>;

public record AccrualsPagedResponse(
    List<AccrualSummaryDto> Items,
    int TotalCount,
    int Page,
    int PageSize);

public class GetAccrualsHandler(AccountingDbContext context) 
    : IQueryHandler<GetAccrualsQuery, AccrualsPagedResponse>
{
    public async ValueTask<AccrualsPagedResponse> Handle(GetAccrualsQuery query, CancellationToken ct)
    {
        var queryable = context.Accruals.AsQueryable();
        
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
            .Select(x => new AccrualSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new AccrualsPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
