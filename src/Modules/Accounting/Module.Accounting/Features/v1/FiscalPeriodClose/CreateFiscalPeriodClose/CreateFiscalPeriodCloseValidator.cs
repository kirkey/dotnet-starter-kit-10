using FluentValidation;

namespace FSH.Module.Accounting.Features.v1.FiscalPeriodClose.CreateFiscalPeriodClose;

public class CreateFiscalPeriodCloseValidator : AbstractValidator<CreateFiscalPeriodCloseCommand>
{
    public CreateFiscalPeriodCloseValidator()
    {
        RuleFor(x => x.FiscalPeriodId).NotEmpty();
        RuleFor(x => x.FiscalYear).GreaterThan(1900);
        RuleFor(x => x.PeriodName).NotEmpty().MaximumLength(AccountingStringLengths.Medium);
        RuleFor(x => x.StartDate).NotEmpty();
        RuleFor(x => x.EndDate).NotEmpty();
        RuleFor(x => x.RetainedEarnings).GreaterThanOrEqualTo(0);
    }
}
