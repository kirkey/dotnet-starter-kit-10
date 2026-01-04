using FluentValidation;

namespace FSH.Module.Accounting.Features.v1.BankReconciliations.ApproveBankReconciliation;

public class ApproveBankReconciliationValidator : AbstractValidator<ApproveBankReconciliationCommand>
{
    public ApproveBankReconciliationValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
