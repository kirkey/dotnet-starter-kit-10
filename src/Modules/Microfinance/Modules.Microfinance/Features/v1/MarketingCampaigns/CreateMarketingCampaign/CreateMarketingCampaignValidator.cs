namespace FSH.Modules.Microfinance.Features.v1.MarketingCampaigns.CreateMarketingCampaign;

public class CreateMarketingCampaignValidator : AbstractValidator<CreateMarketingCampaignCommand>
{
    public CreateMarketingCampaignValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
    }
}
