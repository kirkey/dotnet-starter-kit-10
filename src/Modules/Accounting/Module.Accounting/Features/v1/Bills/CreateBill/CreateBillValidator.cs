using FluentValidation;
using FSH.Module.Accounting.Contracts.v1.Bills.CreateBill;

namespace FSH.Module.Accounting.Features.v1.Bills.CreateBill;

public class CreateBillValidator : AbstractValidator<CreateBillCommand>
{
    public CreateBillValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(AccountingStringLengths.Name);
            
        When(x => !string.IsNullOrEmpty(x.Description), () =>
        {
            RuleFor(x => x.Description)
                .MaximumLength(AccountingStringLengths.Description);
        });
    }
}
