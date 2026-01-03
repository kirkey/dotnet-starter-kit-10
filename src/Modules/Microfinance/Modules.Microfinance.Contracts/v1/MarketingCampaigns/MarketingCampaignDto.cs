namespace FSH.Modules.Microfinance.Contracts.v1.MarketingCampaigns;

public record MarketingCampaignDto(
    Guid Id,
    string Name,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record MarketingCampaignSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
