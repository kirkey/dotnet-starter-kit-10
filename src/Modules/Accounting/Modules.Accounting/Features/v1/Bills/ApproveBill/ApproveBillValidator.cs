using FluentValidation;

namespace FSH.Modules.Accounting.Features.v1.Bills.ApproveBill;

public class ApproveBillValidator : AbstractValidator<ApproveBillCommand>
{
    public ApproveBillValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
