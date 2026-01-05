using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.MarketingCampaigns.CreateMarketingCampaign;

public sealed record CreateMarketingCampaignCommand(string Name) : ICommand<Guid>;
