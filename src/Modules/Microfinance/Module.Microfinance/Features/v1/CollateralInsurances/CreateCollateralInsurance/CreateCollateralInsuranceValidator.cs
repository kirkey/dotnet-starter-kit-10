using FSH.Module.Microfinance.Contracts.v1.CollateralInsurances.CreateCollateralInsurance;

namespace FSH.Module.Microfinance.Features.v1.CollateralInsurances.CreateCollateralInsurance;

public class CreateCollateralInsuranceValidator : AbstractValidator<CreateCollateralInsuranceCommand>
{
    public CreateCollateralInsuranceValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
    }
}
