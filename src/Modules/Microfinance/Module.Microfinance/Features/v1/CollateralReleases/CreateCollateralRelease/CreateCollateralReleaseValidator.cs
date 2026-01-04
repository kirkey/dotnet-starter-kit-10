namespace FSH.Module.Microfinance.Features.v1.CollateralReleases.CreateCollateralRelease;

public class CreateCollateralReleaseValidator : AbstractValidator<CreateCollateralReleaseCommand>
{
    public CreateCollateralReleaseValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
    }
}
