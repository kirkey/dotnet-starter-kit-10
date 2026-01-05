using FluentValidation;
using FSH.Module.Accounting.Contracts.v1.Payments.UpdatePayment;

namespace FSH.Module.Accounting.Features.v1.Payments.UpdatePayment;

public class UpdatePaymentValidator : AbstractValidator<UpdatePaymentCommand>
{
    public UpdatePaymentValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(AccountingStringLengths.Name);
            
        When(x => !string.IsNullOrEmpty(x.Description), () =>
        {
            RuleFor(x => x.Description)
                .MaximumLength(AccountingStringLengths.Description);
        });
    }
}
