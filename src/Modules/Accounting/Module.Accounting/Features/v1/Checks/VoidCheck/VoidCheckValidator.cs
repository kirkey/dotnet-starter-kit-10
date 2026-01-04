using FluentValidation;

namespace FSH.Module.Accounting.Features.v1.Checks.VoidCheck;

public class VoidCheckValidator : AbstractValidator<VoidCheckCommand>
{
    public VoidCheckValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
