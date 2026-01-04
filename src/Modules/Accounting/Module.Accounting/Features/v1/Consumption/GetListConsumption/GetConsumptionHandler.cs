using FSH.Module.Accounting.Contracts.v1.Consumption;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.Consumption.GetListConsumption;

public record GetConsumptionQuery(
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    bool? IsActive = null) : IQuery<ConsumptionPagedResponse>;

public record ConsumptionPagedResponse(
    List<ConsumptionSummaryDto> Items,
    int TotalCount,
    int Page,
    int PageSize);

public class GetConsumptionHandler(AccountingDbContext context) 
    : IQueryHandler<GetConsumptionQuery, ConsumptionPagedResponse>
{
    public async ValueTask<ConsumptionPagedResponse> Handle(GetConsumptionQuery query, CancellationToken ct)
    {
        var queryable = context.Consumption.AsQueryable();
        
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
            .Select(x => new ConsumptionSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new ConsumptionPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
