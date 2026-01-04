using FluentValidation;

namespace FSH.Module.Accounting.Features.v1.BankReconciliations.ExportBankReconciliation;

public class ExportBankReconciliationValidator : AbstractValidator<ExportBankReconciliationQuery>
{
    public ExportBankReconciliationValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Format).NotEmpty();
    }
}
