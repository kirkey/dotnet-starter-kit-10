using FluentValidation;

namespace FSH.Modules.Accounting.Features.v1.BankReconciliations.ExportBankReconciliation;

public class ExportBankReconciliationValidator : AbstractValidator<ExportBankReconciliationCommand>
{
    public ExportBankReconciliationValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
