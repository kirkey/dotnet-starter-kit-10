using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.MarketingCampaigns.GetMarketingCampaigns;

public sealed record GetMarketingCampaignsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<MarketingCampaignsPagedResponse>;
