using FluentValidation;
using FSH.Module.Accounting.Contracts.v1.Invoices.UpdateInvoice;

namespace FSH.Module.Accounting.Features.v1.Invoices.UpdateInvoice;

public class UpdateInvoiceValidator : AbstractValidator<UpdateInvoiceCommand>
{
    public UpdateInvoiceValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        
        RuleFor(x => x.InvoiceNumber)
            .NotEmpty()
            .MaximumLength(AccountingStringLengths.InvoiceNumber);
            
        RuleFor(x => x.InvoiceDate)
            .NotEmpty();
            
        RuleFor(x => x.DueDate)
            .NotEmpty();
            
        RuleFor(x => x.BillToName)
            .NotEmpty()
            .MaximumLength(AccountingStringLengths.Name);
            
        RuleFor(x => x.ExchangeRate)
            .GreaterThan(0).WithMessage("Exchange rate must be positive");
            
        When(x => !string.IsNullOrEmpty(x.BillToAddress), () =>
        {
            RuleFor(x => x.BillToAddress)
                .MaximumLength(AccountingStringLengths.Description);
        });
            
        When(x => !string.IsNullOrEmpty(x.Description), () =>
        {
            RuleFor(x => x.Description)
                .MaximumLength(AccountingStringLengths.Description);
        });
    }
}
