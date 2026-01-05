using FluentValidation;
using FSH.Module.Accounting.Contracts.v1.RetainedEarnings.ReopenRetainedEarnings;

namespace FSH.Module.Accounting.Features.v1.RetainedEarnings.ReopenRetainedEarnings;

public class ReopenRetainedEarningsValidator : AbstractValidator<ReopenRetainedEarningsCommand>
{
    public ReopenRetainedEarningsValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
