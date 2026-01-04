using FluentValidation;
using FSH.Module.Accounting.Contracts.v1.Checks.StopPaymentCheck;

namespace FSH.Module.Accounting.Features.v1.Checks.StopPaymentCheck;

public class StopPaymentCheckValidator : AbstractValidator<StopPaymentCheckCommand>
{
    public StopPaymentCheckValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
