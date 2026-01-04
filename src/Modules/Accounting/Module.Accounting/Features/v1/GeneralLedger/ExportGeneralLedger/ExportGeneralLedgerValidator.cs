using FluentValidation;

namespace FSH.Module.Accounting.Features.v1.GeneralLedger.ExportGeneralLedger;

public class ExportGeneralLedgerValidator : AbstractValidator<ExportGeneralLedgerQuery>
{
    public ExportGeneralLedgerValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Format).NotEmpty();
    }
}
