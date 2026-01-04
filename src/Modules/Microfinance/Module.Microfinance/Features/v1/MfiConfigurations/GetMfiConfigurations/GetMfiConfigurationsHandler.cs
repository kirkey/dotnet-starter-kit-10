using FSH.Module.Microfinance.Contracts.v1.MfiConfigurations;
using FSH.Module.Microfinance.Data;

namespace FSH.Module.Microfinance.Features.v1.MfiConfigurations.GetMfiConfigurations;

public record GetMfiConfigurationsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<MfiConfigurationsPagedResponse>;

public class GetMfiConfigurationsHandler(MicrofinanceDbContext context) : IQueryHandler<GetMfiConfigurationsQuery, MfiConfigurationsPagedResponse>
{
    public async ValueTask<MfiConfigurationsPagedResponse> Handle(GetMfiConfigurationsQuery query, CancellationToken ct)
    {
        var queryable = context.MfiConfigurations.AsQueryable();
        
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
            .Select(x => new MfiConfigurationSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new MfiConfigurationsPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
