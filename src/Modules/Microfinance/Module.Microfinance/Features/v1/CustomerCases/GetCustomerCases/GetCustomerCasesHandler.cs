using FSH.Module.Microfinance.Contracts.v1.CustomerCases;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.CustomerCases.GetCustomerCases;

namespace FSH.Module.Microfinance.Features.v1.CustomerCases.GetCustomerCases;

public class GetCustomerCasesHandler(MicrofinanceDbContext context) : IQueryHandler<GetCustomerCasesQuery, CustomerCasesPagedResponse>
{
    public async ValueTask<CustomerCasesPagedResponse> Handle(GetCustomerCasesQuery query, CancellationToken ct)
    {
        var queryable = context.CustomerCases.AsQueryable();
        
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
            .Select(x => new CustomerCaseSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new CustomerCasesPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
