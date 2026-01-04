using FluentValidation;

namespace FSH.Module.Accounting.Features.v1.RetainedEarnings.ReopenRetainedEarnings;

public class ReopenRetainedEarningsValidator : AbstractValidator<ReopenRetainedEarningsCommand>
{
    public ReopenRetainedEarningsValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
