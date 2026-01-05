using FSH.Module.Microfinance.Contracts.v1.InterestRateChanges.CreateInterestRateChange;

namespace FSH.Module.Microfinance.Features.v1.InterestRateChanges.CreateInterestRateChange;

public class CreateInterestRateChangeValidator : AbstractValidator<CreateInterestRateChangeCommand>
{
    public CreateInterestRateChangeValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
    }
}
