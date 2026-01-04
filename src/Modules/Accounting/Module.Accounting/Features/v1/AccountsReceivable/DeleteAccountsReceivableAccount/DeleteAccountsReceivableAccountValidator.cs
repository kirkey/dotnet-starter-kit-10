using FluentValidation;

namespace FSH.Module.Accounting.Features.v1.AccountsReceivable.DeleteAccountsReceivableAccount;

public class DeleteAccountsReceivableAccountValidator : AbstractValidator<DeleteAccountsReceivableAccountCommand>
{
    public DeleteAccountsReceivableAccountValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
