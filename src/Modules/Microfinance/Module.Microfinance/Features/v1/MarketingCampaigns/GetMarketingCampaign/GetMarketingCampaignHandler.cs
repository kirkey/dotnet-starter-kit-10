using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Contracts.v1.MarketingCampaigns;
using FSH.Module.Microfinance.Data;

namespace FSH.Module.Microfinance.Features.v1.MarketingCampaigns.GetMarketingCampaign;

public record GetMarketingCampaignQuery(Guid Id) : IQuery<MarketingCampaignDto>;

public class GetMarketingCampaignHandler(MicrofinanceDbContext context) : IQueryHandler<GetMarketingCampaignQuery, MarketingCampaignDto>
{
    public async ValueTask<MarketingCampaignDto> Handle(GetMarketingCampaignQuery query, CancellationToken ct)
    {
        var entity = await context.MarketingCampaigns
            .Where(x => x.Id == query.Id)
            .Select(x => new MarketingCampaignDto(x.Id, x.Name, x.IsActive, x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("MarketingCampaign not found");
    }
}
