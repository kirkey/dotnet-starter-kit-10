using FSH.Modules.Microfinance.Contracts.v1.CustomerSegments;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.CustomerSegments.GetCustomerSegments;

public record GetCustomerSegmentsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<CustomerSegmentsPagedResponse>;

public class GetCustomerSegmentsHandler(MicrofinanceDbContext context) : IQueryHandler<GetCustomerSegmentsQuery, CustomerSegmentsPagedResponse>
{
    public async ValueTask<CustomerSegmentsPagedResponse> Handle(GetCustomerSegmentsQuery query, CancellationToken ct)
    {
        var queryable = context.CustomerSegments.AsQueryable();
        
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
            .Select(x => new CustomerSegmentSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new CustomerSegmentsPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
