using FSH.Module.Accounting.Contracts.v1.Meters;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

using FSH.Module.Accounting.Contracts.v1.Meters.GetListMeter;

namespace FSH.Module.Accounting.Features.v1.Meters.GetMeters;

public class GetMetersHandler(AccountingDbContext context) 
    : IQueryHandler<GetMetersQuery, MetersPagedResponse>
{
    public async ValueTask<MetersPagedResponse> Handle(GetMetersQuery query, CancellationToken ct)
    {
        var queryable = context.Meters.AsQueryable();
        
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
            .Select(x => new MeterSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new MetersPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
