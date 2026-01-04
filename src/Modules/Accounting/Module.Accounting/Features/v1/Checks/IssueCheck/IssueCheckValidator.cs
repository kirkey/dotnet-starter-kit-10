using FluentValidation;
using FSH.Module.Accounting.Contracts.v1.Checks.IssueCheck;

namespace FSH.Module.Accounting.Features.v1.Checks.IssueCheck;

public class IssueCheckValidator : AbstractValidator<IssueCheckCommand>
{
    public IssueCheckValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
