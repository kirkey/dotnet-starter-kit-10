using FSH.Module.Microfinance.Contracts.v1.FeeWaivers.CreateFeeWaiver;

namespace FSH.Module.Microfinance.Features.v1.FeeWaivers.CreateFeeWaiver;

public class CreateFeeWaiverValidator : AbstractValidator<CreateFeeWaiverCommand>
{
    public CreateFeeWaiverValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
    }
}
