using FluentValidation;
using FSH.Module.Accounting.Contracts.v1.AccountsReceivable.UpdateAccountsReceivableAccount;

namespace FSH.Module.Accounting.Features.v1.AccountsReceivable.UpdateAccountsReceivableAccount;

public class UpdateAccountsReceivableAccountValidator : AbstractValidator<UpdateAccountsReceivableAccountCommand>
{
    public UpdateAccountsReceivableAccountValidator()
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
