using FluentValidation;

namespace FSH.Module.Accounting.Features.v1.Checks.PrintCheck;

public class PrintCheckValidator : AbstractValidator<PrintCheckCommand>
{
    public PrintCheckValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
