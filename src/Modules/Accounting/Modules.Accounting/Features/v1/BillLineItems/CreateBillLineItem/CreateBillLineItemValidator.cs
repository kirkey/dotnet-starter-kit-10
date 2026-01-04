using FluentValidation;

namespace FSH.Modules.Accounting.Features.v1.BillLineItems.CreateBillLineItem;

public class CreateBillLineItemValidator : AbstractValidator<CreateBillLineItemCommand>
{
    public CreateBillLineItemValidator()
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
