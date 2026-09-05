using FluentValidation;
using FSH.Modules.Billing.Contracts.v1.Invoices;

namespace FSH.Modules.Billing.Features.v1.Invoices.MarkInvoicePaid;

public sealed class MarkInvoicePaidCommandValidator : AbstractValidator<MarkInvoicePaidCommand>
{
    public MarkInvoicePaidCommandValidator()
    {
        RuleFor(x => x.InvoiceId).NotEmpty();
    }
}
