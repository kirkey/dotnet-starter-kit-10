using FluentValidation;
using FSH.Modules.Billing.Contracts.v1.Invoices;

namespace FSH.Modules.Billing.Features.v1.Invoices.VoidInvoice;

public sealed class VoidInvoiceCommandValidator : AbstractValidator<VoidInvoiceCommand>
{
    public VoidInvoiceCommandValidator()
    {
        RuleFor(x => x.InvoiceId).NotEmpty();
        RuleFor(x => x.Reason).MaximumLength(512);
    }
}
