using FSH.Module.Microfinance.Contracts.v1.RiskIndicators.CreateRiskIndicator;

namespace FSH.Module.Microfinance.Features.v1.RiskIndicators.CreateRiskIndicator;

public class CreateRiskIndicatorValidator : AbstractValidator<CreateRiskIndicatorCommand>
{
    public CreateRiskIndicatorValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
    }
}
