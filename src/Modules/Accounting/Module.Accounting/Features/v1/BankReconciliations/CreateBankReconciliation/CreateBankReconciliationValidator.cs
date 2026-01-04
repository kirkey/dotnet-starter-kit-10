using FluentValidation;

namespace FSH.Module.Accounting.Features.v1.BankReconciliations.CreateBankReconciliation;

public class CreateBankReconciliationValidator : AbstractValidator<CreateBankReconciliationCommand>
{
    public CreateBankReconciliationValidator()
    {
        RuleFor(x => x.ReconciliationNumber).NotEmpty().MaximumLength(AccountingStringLengths.Medium);
        RuleFor(x => x.BankAccountId).NotEmpty();
        RuleFor(x => x.StatementDate).NotEmpty();
        RuleFor(x => x.StatementBalance).GreaterThanOrEqualTo(0);
        RuleFor(x => x.BookBalance).GreaterThanOrEqualTo(0);
    }
}
