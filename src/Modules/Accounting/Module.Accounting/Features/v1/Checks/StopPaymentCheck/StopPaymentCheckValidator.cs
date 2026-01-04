using FluentValidation;

namespace FSH.Module.Accounting.Features.v1.Checks.StopPaymentCheck;

public class StopPaymentCheckValidator : AbstractValidator<StopPaymentCheckCommand>
{
    public StopPaymentCheckValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
