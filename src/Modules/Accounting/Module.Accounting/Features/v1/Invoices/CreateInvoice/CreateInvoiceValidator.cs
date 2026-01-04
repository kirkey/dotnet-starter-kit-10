using FluentValidation;

namespace FSH.Module.Accounting.Features.v1.Invoices.CreateInvoice;

public class CreateInvoiceValidator : AbstractValidator<CreateInvoiceCommand>
{
    public CreateInvoiceValidator()
    {
        RuleFor(x => x.InvoiceNumber)
            .NotEmpty()
            .MaximumLength(AccountingStringLengths.InvoiceNumber);
            
        RuleFor(x => x.InvoiceDate)
            .NotEmpty();
            
        RuleFor(x => x.InvoiceType)
            .NotEmpty()
            .Must(BeValidInvoiceType).WithMessage("InvoiceType must be AR or AP");
            
        RuleFor(x => x.DueDate)
            .NotEmpty();
            
        RuleFor(x => x.BillToName)
            .NotEmpty()
            .MaximumLength(AccountingStringLengths.Name);
            
        RuleFor(x => x.ExchangeRate)
            .GreaterThan(0).WithMessage("Exchange rate must be positive");
            
        When(x => x.InvoiceType == "AR", () =>
        {
            RuleFor(x => x.CustomerId)
                .NotEmpty().WithMessage("Customer is required for AR invoices");
        });
            
        When(x => x.InvoiceType == "AP", () =>
        {
            RuleFor(x => x.VendorId)
                .NotEmpty().WithMessage("Vendor is required for AP invoices");
        });
            
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
    
    private bool BeValidInvoiceType(string invoiceType)
    {
        return invoiceType.Equals("AR", StringComparison.OrdinalIgnoreCase) ||
               invoiceType.Equals("AP", StringComparison.OrdinalIgnoreCase);
    }
}
