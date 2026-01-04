namespace FSH.Module.Microfinance.Contracts.v1.MarketingCampaigns;

public record GetMarketingCampaignQuery(Guid Id);
public record GetMarketingCampaignsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive);
public record MarketingCampaignsPagedResponse(List<MarketingCampaignSummaryDto> Items, int TotalCount, int Page, int PageSize);
