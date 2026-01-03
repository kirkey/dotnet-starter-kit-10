namespace FSH.Modules.Microfinance.Features.v1.CollateralValuations.CreateCollateralValuation;

public class CreateCollateralValuationValidator : AbstractValidator<CreateCollateralValuationCommand>
{
    public CreateCollateralValuationValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
    }
}
