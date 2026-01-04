using FluentValidation;

namespace FSH.Module.Accounting.Features.v1.Payments.ApprovePayment;

public class ApprovePaymentValidator : AbstractValidator<ApprovePaymentCommand>
{
    public ApprovePaymentValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
