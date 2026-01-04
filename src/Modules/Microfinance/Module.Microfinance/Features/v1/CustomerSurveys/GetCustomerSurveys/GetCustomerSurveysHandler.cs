using FSH.Module.Microfinance.Contracts.v1.CustomerSurveys;
using FSH.Module.Microfinance.Data;

namespace FSH.Module.Microfinance.Features.v1.CustomerSurveys.GetCustomerSurveys;

public record GetCustomerSurveysQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<CustomerSurveysPagedResponse>;

public class GetCustomerSurveysHandler(MicrofinanceDbContext context) : IQueryHandler<GetCustomerSurveysQuery, CustomerSurveysPagedResponse>
{
    public async ValueTask<CustomerSurveysPagedResponse> Handle(GetCustomerSurveysQuery query, CancellationToken ct)
    {
        var queryable = context.CustomerSurveys.AsQueryable();
        
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
            .Select(x => new CustomerSurveySummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new CustomerSurveysPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
