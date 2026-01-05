using FluentValidation;
using FSH.Module.Accounting.Contracts.v1.BillLineItems.UpdateBillLineItem;

namespace FSH.Module.Accounting.Features.v1.BillLineItems.UpdateBillLineItem;

public class UpdateBillLineItemValidator : AbstractValidator<UpdateBillLineItemCommand>
{
    public UpdateBillLineItemValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
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
