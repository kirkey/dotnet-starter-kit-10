using FluentValidation;
using FSH.Module.Accounting.Contracts.v1.Checks.VoidCheck;

namespace FSH.Module.Accounting.Features.v1.Checks.VoidCheck;

public class VoidCheckValidator : AbstractValidator<VoidCheckCommand>
{
    public VoidCheckValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
