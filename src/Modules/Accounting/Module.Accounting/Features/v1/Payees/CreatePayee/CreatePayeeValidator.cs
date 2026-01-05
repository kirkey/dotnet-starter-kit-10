using FluentValidation;
using FSH.Module.Accounting.Contracts.v1.Payees.CreatePayee;

namespace FSH.Module.Accounting.Features.v1.Payees.CreatePayee;

public class CreatePayeeValidator : AbstractValidator<CreatePayeeCommand>
{
    public CreatePayeeValidator()
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
