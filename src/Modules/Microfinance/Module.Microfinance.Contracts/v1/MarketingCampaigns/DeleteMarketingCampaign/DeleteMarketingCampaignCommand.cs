using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.MarketingCampaigns.DeleteMarketingCampaign;

public sealed record DeleteMarketingCampaignCommand(Guid Id) : ICommand;
