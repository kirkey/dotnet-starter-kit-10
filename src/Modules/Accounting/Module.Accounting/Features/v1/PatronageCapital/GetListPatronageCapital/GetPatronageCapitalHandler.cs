using FSH.Module.Accounting.Contracts.v1.PatronageCapital;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

using FSH.Module.Accounting.Contracts.v1.PatronageCapital.GetListPatronageCapital;namespace FSH.Module.Accounting.Features.v1.PatronageCapital.GetListPatronageCapital;

public class GetPatronageCapitalHandler(AccountingDbContext context) 
    : IQueryHandler<GetPatronageCapitalQuery, PatronageCapitalPagedResponse>
{
    public async ValueTask<PatronageCapitalPagedResponse> Handle(GetPatronageCapitalQuery query, CancellationToken ct)
    {
        var queryable = context.PatronageCapital.AsQueryable();
        
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
            .Select(x => new PatronageCapitalSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new PatronageCapitalPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
