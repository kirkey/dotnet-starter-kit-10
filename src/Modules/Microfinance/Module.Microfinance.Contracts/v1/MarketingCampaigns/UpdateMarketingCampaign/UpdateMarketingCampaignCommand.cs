using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.MarketingCampaigns.UpdateMarketingCampaign;

public sealed record UpdateMarketingCampaignCommand(Guid Id, string Name) : ICommand<Guid>;
