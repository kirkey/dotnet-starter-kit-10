using FSH.Module.Microfinance.Contracts.v1.MarketingCampaigns.CreateMarketingCampaign;

namespace FSH.Module.Microfinance.Features.v1.MarketingCampaigns.CreateMarketingCampaign;

public class CreateMarketingCampaignValidator : AbstractValidator<CreateMarketingCampaignCommand>
{
    public CreateMarketingCampaignValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
    }
}
