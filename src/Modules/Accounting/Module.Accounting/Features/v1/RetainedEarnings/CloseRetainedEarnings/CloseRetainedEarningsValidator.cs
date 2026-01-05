using FluentValidation;
using FSH.Module.Accounting.Contracts.v1.RetainedEarnings.CloseRetainedEarnings;

namespace FSH.Module.Accounting.Features.v1.RetainedEarnings.CloseRetainedEarnings;

public class CloseRetainedEarningsValidator : AbstractValidator<CloseRetainedEarningsCommand>
{
    public CloseRetainedEarningsValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.FiscalYear).GreaterThan(0);
        RuleFor(x => x.ClosingBalance).GreaterThanOrEqualTo(0);
    }
}
