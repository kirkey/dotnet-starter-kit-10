using FluentValidation;
using FSH.Module.Accounting.Contracts.v1.Payments.CreatePayment;
using FSH.Module.Accounting.Features;

namespace FSH.Module.Accounting.Features.v1.Payments.CreatePayment;

public class CreatePaymentValidator : AbstractValidator<CreatePaymentCommand>
{
    public CreatePaymentValidator()
    {
        RuleFor(x => x.Name)
            .ValidateName();
            
        RuleFor(x => x.Description)
            .ValidateDescription();
    }
}
