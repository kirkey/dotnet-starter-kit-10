using FluentValidation;

namespace FSH.Module.Accounting.Features.v1.GeneralLedger.ExportGeneralLedger;

public class ExportGeneralLedgerValidator : AbstractValidator<ExportGeneralLedgerCommand>
{
    public ExportGeneralLedgerValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
