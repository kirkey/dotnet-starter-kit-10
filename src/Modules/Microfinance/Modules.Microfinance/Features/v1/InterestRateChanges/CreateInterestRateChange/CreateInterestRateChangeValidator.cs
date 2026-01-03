namespace FSH.Modules.Microfinance.Features.v1.InterestRateChanges.CreateInterestRateChange;

public class CreateInterestRateChangeValidator : AbstractValidator<CreateInterestRateChangeCommand>
{
    public CreateInterestRateChangeValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
    }
}
