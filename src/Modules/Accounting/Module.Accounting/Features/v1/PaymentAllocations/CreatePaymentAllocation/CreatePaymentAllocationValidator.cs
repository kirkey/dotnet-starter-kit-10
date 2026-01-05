using FluentValidation;
using FSH.Module.Accounting.Contracts.v1.PaymentAllocations.CreatePaymentAllocation;

namespace FSH.Module.Accounting.Features.v1.PaymentAllocations.CreatePaymentAllocation;

public class CreatePaymentAllocationValidator : AbstractValidator<CreatePaymentAllocationCommand>
{
    public CreatePaymentAllocationValidator()
    {
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
