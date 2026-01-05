using FSH.Module.Microfinance.Contracts.v1.LoanSchedules;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.LoanSchedules.GetLoanSchedules;

namespace FSH.Module.Microfinance.Features.v1.LoanSchedules.GetLoanSchedules;

public class GetLoanSchedulesHandler(MicrofinanceDbContext context) : IQueryHandler<GetLoanSchedulesQuery, LoanSchedulesPagedResponse>
{
    public async ValueTask<LoanSchedulesPagedResponse> Handle(GetLoanSchedulesQuery query, CancellationToken ct)
    {
        var queryable = context.LoanSchedules.AsQueryable();
        
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
            .Select(x => new LoanScheduleSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new LoanSchedulesPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
