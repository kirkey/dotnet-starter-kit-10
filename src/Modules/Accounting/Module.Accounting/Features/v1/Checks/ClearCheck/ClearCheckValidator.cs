using FluentValidation;

namespace FSH.Module.Accounting.Features.v1.Checks.ClearCheck;

public class ClearCheckValidator : AbstractValidator<ClearCheckCommand>
{
    public ClearCheckValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
