using FluentValidation;
using FSH.Module.Accounting.Contracts.v1.BankReconciliations.ApproveBankReconciliation;

namespace FSH.Module.Accounting.Features.v1.BankReconciliations.ApproveBankReconciliation;

public class ApproveBankReconciliationValidator : AbstractValidator<ApproveBankReconciliationCommand>
{
    public ApproveBankReconciliationValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
