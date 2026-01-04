using FluentValidation;

namespace FSH.Modules.Accounting.Features.v1.Checks.IssueCheck;

public class IssueCheckValidator : AbstractValidator<IssueCheckCommand>
{
    public IssueCheckValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
