using FSH.Module.Accounting.Contracts.v1.Accruals;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

using FSH.Module.Accounting.Contracts.v1.Accruals.GetListAccrual;namespace FSH.Module.Accounting.Features.v1.Accruals.GetAccruals;

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
