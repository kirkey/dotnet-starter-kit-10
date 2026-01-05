using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.Staffs.GetStaffs;
using FSH.Module.Microfinance.Contracts.v1.Staffs;

namespace FSH.Module.Microfinance.Features.v1.Staffs.GetStaffs;

public class GetStaffsHandler(MicrofinanceDbContext context) : IQueryHandler<GetStaffsQuery, StaffsPagedResponse>
{
    public async ValueTask<StaffsPagedResponse> Handle(GetStaffsQuery query, CancellationToken ct)
    {
        var queryable = context.Staffs.AsQueryable();
        
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
            .Select(x => new StaffSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new StaffsPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
