using FSH.Module.Accounting.Contracts.v1.Payees;
using FSH.Module.Accounting.Contracts.v1.Payees.GetPayees;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.Payees.GetPayees;

public class GetPayeesHandler(AccountingDbContext context) 
    : IQueryHandler<GetPayeesQuery, PayeesPagedResponse>
{
    public async ValueTask<PayeesPagedResponse> Handle(GetPayeesQuery query, CancellationToken ct)
    {
        var queryable = context.Payees.AsQueryable();
        
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
            .Select(x => new PayeeSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new PayeesPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
