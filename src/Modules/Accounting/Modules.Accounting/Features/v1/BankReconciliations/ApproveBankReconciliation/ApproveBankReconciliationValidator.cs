using FluentValidation;

namespace FSH.Modules.Accounting.Features.v1.BankReconciliations.ApproveBankReconciliation;

public class ApproveBankReconciliationValidator : AbstractValidator<ApproveBankReconciliationCommand>
{
    public ApproveBankReconciliationValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
