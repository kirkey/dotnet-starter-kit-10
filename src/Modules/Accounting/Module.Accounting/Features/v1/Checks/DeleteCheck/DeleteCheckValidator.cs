using FluentValidation;

namespace FSH.Module.Accounting.Features.v1.Checks.DeleteCheck;

public class DeleteCheckValidator : AbstractValidator<DeleteCheckCommand>
{
    public DeleteCheckValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
