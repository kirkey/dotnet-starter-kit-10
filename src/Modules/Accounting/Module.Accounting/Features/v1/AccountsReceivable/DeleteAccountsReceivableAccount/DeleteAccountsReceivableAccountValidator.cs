using FluentValidation;
using FSH.Module.Accounting.Contracts.v1.AccountsReceivable.DeleteAccountsReceivableAccount;

namespace FSH.Module.Accounting.Features.v1.AccountsReceivable.DeleteAccountsReceivableAccount;

public class DeleteAccountsReceivableAccountValidator : AbstractValidator<DeleteAccountsReceivableAccountCommand>
{
    public DeleteAccountsReceivableAccountValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
