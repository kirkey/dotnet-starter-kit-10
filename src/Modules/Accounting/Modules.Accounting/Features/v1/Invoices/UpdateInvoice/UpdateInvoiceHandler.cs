using FSH.Framework.Core.Exceptions;
using FSH.Modules.Accounting.Data;
using Mediator;

namespace FSH.Modules.Accounting.Features.v1.Invoices.UpdateInvoice;

public record UpdateInvoiceCommand(
    Guid Id,
    string InvoiceNumber,
    DateTime InvoiceDate,
    DateTime DueDate,
    string BillToName,
    Guid? CustomerId = null,
    Guid? VendorId = null,
    string? BillToAddress = null,
    string? ShipToName = null,
    string? ShipToAddress = null,
    string? PaymentTerms = null,
    string? TaxCode = null,
    string? CurrencyCode = null,
    decimal ExchangeRate = 1.0m,
    string? ReferenceNumber = null,
    string? PurchaseOrderNumber = null,
    string? Description = null,
    string? Notes = null,
    string? Terms = null) : ICommand<Guid>;

public class UpdateInvoiceHandler(AccountingDbContext context) : ICommandHandler<UpdateInvoiceCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateInvoiceCommand command, CancellationToken ct)
    {
        var entity = await context.Invoices.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("Invoice not found");
        
        entity.Update(
            command.InvoiceNumber,
            command.InvoiceDate,
            command.DueDate,
            command.BillToName,
            command.CustomerId,
            command.VendorId,
            command.BillToAddress,
            command.ShipToName,
            command.ShipToAddress,
            command.PaymentTerms,
            command.TaxCode,
            command.CurrencyCode,
            command.ExchangeRate,
            command.ReferenceNumber,
            command.PurchaseOrderNumber,
            command.Description,
            command.Notes,
            command.Terms);
        
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
