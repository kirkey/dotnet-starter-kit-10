using FluentValidation;

namespace FSH.Module.Accounting.Features.v1.BankReconciliations.AddBankReconciliationLine;

public class AddBankReconciliationLineValidator : AbstractValidator<AddBankReconciliationLineCommand>
{
    public AddBankReconciliationLineValidator()
    {
        RuleFor(x => x.BankReconciliationId).NotEmpty();
        RuleFor(x => x.TransactionId).NotEmpty();
        RuleFor(x => x.TransactionDate).NotEmpty();
        RuleFor(x => x.Amount).GreaterThan(0);
    }
}