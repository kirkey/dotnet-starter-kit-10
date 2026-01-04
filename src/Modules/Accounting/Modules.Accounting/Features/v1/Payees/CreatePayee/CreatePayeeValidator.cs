using FluentValidation;

namespace FSH.Modules.Accounting.Features.v1.Payees.CreatePayee;

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
