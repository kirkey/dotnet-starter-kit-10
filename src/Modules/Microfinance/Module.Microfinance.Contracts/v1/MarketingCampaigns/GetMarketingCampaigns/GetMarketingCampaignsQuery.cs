using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.MarketingCampaigns.GetMarketingCampaigns;

public sealed record GetMarketingCampaignsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<MarketingCampaignsPagedResponse>;

public sealed record MarketingCampaignsPagedResponse(List<MarketingCampaignSummaryDto> Items, int TotalCount, int Page, int PageSize);
