using FSH.Modules.Accounting.Contracts.v1.RateSchedules;
using FSH.Modules.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Accounting.Features.v1.RateSchedules.GetRateSchedules;

public record GetRateSchedulesQuery(
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    bool? IsActive = null) : IQuery<RateSchedulesPagedResponse>;

public record RateSchedulesPagedResponse(
    List<RateScheduleSummaryDto> Items,
    int TotalCount,
    int Page,
    int PageSize);

public class GetRateSchedulesHandler(AccountingDbContext context) 
    : IQueryHandler<GetRateSchedulesQuery, RateSchedulesPagedResponse>
{
    public async ValueTask<RateSchedulesPagedResponse> Handle(GetRateSchedulesQuery query, CancellationToken ct)
    {
        var queryable = context.RateSchedules.AsQueryable();
        
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
            .Select(x => new RateScheduleSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new RateSchedulesPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
