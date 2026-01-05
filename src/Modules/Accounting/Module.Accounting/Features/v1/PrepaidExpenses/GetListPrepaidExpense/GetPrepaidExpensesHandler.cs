using FSH.Module.Accounting.Contracts.v1.PrepaidExpenses;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

using FSH.Module.Accounting.Contracts.v1.PrepaidExpenses.GetListPrepaidExpense;

namespace FSH.Module.Accounting.Features.v1.PrepaidExpenses.GetPrepaidExpenses;

public class GetPrepaidExpensesHandler(AccountingDbContext context) 
    : IQueryHandler<GetPrepaidExpensesQuery, PrepaidExpensesPagedResponse>
{
    public async ValueTask<PrepaidExpensesPagedResponse> Handle(GetPrepaidExpensesQuery query, CancellationToken ct)
    {
        var queryable = context.PrepaidExpenses.AsQueryable();
        
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
            .Select(x => new PrepaidExpenseSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new PrepaidExpensesPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
