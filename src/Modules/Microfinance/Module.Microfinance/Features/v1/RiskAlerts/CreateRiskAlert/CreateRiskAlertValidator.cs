using FSH.Module.Microfinance.Contracts.v1.RiskAlerts.CreateRiskAlert;

namespace FSH.Module.Microfinance.Features.v1.RiskAlerts.CreateRiskAlert;

public class CreateRiskAlertValidator : AbstractValidator<CreateRiskAlertCommand>
{
    public CreateRiskAlertValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
    }
}
