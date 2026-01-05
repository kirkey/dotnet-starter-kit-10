using FSH.Module.Accounting.Contracts.v1.AccountingPeriods;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

using FSH.Module.Accounting.Contracts.v1.AccountingPeriods.GetListAccountingPeriod;

namespace FSH.Module.Accounting.Features.v1.AccountingPeriods.GetAccountingPeriods;


public class GetAccountingPeriodsHandler(AccountingDbContext context) 
    : IQueryHandler<GetAccountingPeriodsQuery, AccountingPeriodsPagedResponse>
{
    public async ValueTask<AccountingPeriodsPagedResponse> Handle(GetAccountingPeriodsQuery query, CancellationToken ct)
    {
        var queryable = context.AccountingPeriods.AsQueryable();
        
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
            .Select(x => new AccountingPeriodSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new AccountingPeriodsPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
