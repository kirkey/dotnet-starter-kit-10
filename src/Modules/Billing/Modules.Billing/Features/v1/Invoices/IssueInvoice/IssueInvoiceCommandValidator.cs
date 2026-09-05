using FluentValidation;
using FSH.Modules.Billing.Contracts.v1.Invoices;

namespace FSH.Modules.Billing.Features.v1.Invoices.IssueInvoice;

public sealed class IssueInvoiceCommandValidator : AbstractValidator<IssueInvoiceCommand>
{
    public IssueInvoiceCommandValidator()
    {
        RuleFor(x => x.InvoiceId).NotEmpty();
    }
}
