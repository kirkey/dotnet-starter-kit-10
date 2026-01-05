using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.MarketingCampaigns.GetMarketingCampaign;

public sealed record GetMarketingCampaignQuery(Guid Id) : IQuery<MarketingCampaignDto>;
