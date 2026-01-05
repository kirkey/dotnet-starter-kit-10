using FSH.Module.Microfinance.Contracts.v1.CollateralTypes.CreateCollateralType;

namespace FSH.Module.Microfinance.Features.v1.CollateralTypes.CreateCollateralType;

public class CreateCollateralTypeValidator : AbstractValidator<CreateCollateralTypeCommand>
{
    public CreateCollateralTypeValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
    }
}
