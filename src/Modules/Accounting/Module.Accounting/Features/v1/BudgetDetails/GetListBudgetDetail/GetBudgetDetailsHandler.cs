using FSH.Module.Accounting.Contracts.v1.BudgetDetails.GetListBudgetDetail;
using FSH.Module.Accounting.Contracts.v1.BudgetDetails;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.BudgetDetails.GetListBudgetDetail;



public class GetBudgetDetailsHandler(AccountingDbContext context) 
    : IQueryHandler<GetBudgetDetailsQuery, BudgetDetailsPagedResponse>
{
    public async ValueTask<BudgetDetailsPagedResponse> Handle(GetBudgetDetailsQuery query, CancellationToken ct)
    {
        var queryable = context.BudgetDetails.AsQueryable();
        
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
            .Select(x => new BudgetDetailSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new BudgetDetailsPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
