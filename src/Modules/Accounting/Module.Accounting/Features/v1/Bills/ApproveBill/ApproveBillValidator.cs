using FluentValidation;
using FSH.Module.Accounting.Contracts.v1.Bills.ApproveBill;

namespace FSH.Module.Accounting.Features.v1.Bills.ApproveBill;

public class ApproveBillValidator : AbstractValidator<ApproveBillCommand>
{
    public ApproveBillValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
