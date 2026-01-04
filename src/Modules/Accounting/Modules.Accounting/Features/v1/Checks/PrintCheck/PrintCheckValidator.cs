using FluentValidation;

namespace FSH.Modules.Accounting.Features.v1.Checks.PrintCheck;

public class PrintCheckValidator : AbstractValidator<PrintCheckCommand>
{
    public PrintCheckValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
