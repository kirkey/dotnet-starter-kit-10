using FluentValidation;

namespace FSH.Modules.Accounting.Features.v1.AccountsPayable.CreateAccountsPayableAccount;

public class CreateAccountsPayableAccountValidator : AbstractValidator<CreateAccountsPayableAccountCommand>
{
    public CreateAccountsPayableAccountValidator()
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
