using FSH.Module.Microfinance.Contracts.v1.MarketingCampaigns;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.MarketingCampaigns.GetMarketingCampaigns;

namespace FSH.Module.Microfinance.Features.v1.MarketingCampaigns.GetMarketingCampaigns;

public class GetMarketingCampaignsHandler(MicrofinanceDbContext context) : IQueryHandler<GetMarketingCampaignsQuery, MarketingCampaignsPagedResponse>
{
    public async ValueTask<MarketingCampaignsPagedResponse> Handle(GetMarketingCampaignsQuery query, CancellationToken ct)
    {
        var queryable = context.MarketingCampaigns.AsQueryable();
        
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
            .Select(x => new MarketingCampaignSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new MarketingCampaignsPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
