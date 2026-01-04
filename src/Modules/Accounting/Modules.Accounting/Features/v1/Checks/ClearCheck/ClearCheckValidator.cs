using FluentValidation;

namespace FSH.Modules.Accounting.Features.v1.Checks.ClearCheck;

public class ClearCheckValidator : AbstractValidator<ClearCheckCommand>
{
    public ClearCheckValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
