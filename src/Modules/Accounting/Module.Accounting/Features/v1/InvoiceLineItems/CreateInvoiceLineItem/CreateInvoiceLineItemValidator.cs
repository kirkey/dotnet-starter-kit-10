using FluentValidation;
using FSH.Module.Accounting.Contracts.v1.InvoiceLineItems.CreateInvoiceLineItem;

namespace FSH.Module.Accounting.Features.v1.InvoiceLineItems.CreateInvoiceLineItem;

public class CreateInvoiceLineItemValidator : AbstractValidator<CreateInvoiceLineItemCommand>
{
    public CreateInvoiceLineItemValidator()
    {
        RuleFor(x => x.InvoiceId).NotEmpty();
        RuleFor(x => x.LineNumber).GreaterThan(0);
        RuleFor(x => x.ItemDescription).NotEmpty().MaximumLength(AccountingStringLengths.Description);
        RuleFor(x => x.AccountId).NotEmpty();
        RuleFor(x => x.AccountCode).NotEmpty().MaximumLength(AccountingStringLengths.AccountCode);
        RuleFor(x => x.Quantity).GreaterThan(0);
        RuleFor(x => x.UnitPrice).GreaterThanOrEqualTo(0);
        RuleFor(x => x.DiscountPercent).InclusiveBetween(0, 100);
        RuleFor(x => x.TaxRate).InclusiveBetween(0, 100);
    }
}
