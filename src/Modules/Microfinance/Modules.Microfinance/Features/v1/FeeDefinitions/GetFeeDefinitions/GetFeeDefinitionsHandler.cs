using FSH.Modules.Microfinance.Contracts.v1.FeeDefinitions;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.FeeDefinitions.GetFeeDefinitions;

public record GetFeeDefinitionsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<FeeDefinitionsPagedResponse>;

public class GetFeeDefinitionsHandler(MicrofinanceDbContext context) : IQueryHandler<GetFeeDefinitionsQuery, FeeDefinitionsPagedResponse>
{
    public async ValueTask<FeeDefinitionsPagedResponse> Handle(GetFeeDefinitionsQuery query, CancellationToken ct)
    {
        var queryable = context.FeeDefinitions.AsQueryable();
        
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
            .Select(x => new FeeDefinitionSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new FeeDefinitionsPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
