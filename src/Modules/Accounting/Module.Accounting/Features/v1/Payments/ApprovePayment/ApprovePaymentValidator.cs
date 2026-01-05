using FluentValidation;
using FSH.Module.Accounting.Contracts.v1.Payments.ApprovePayment;

namespace FSH.Module.Accounting.Features.v1.Payments.ApprovePayment;

public class ApprovePaymentValidator : AbstractValidator<ApprovePaymentCommand>
{
    public ApprovePaymentValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
