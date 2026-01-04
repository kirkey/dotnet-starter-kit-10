using FSH.Framework.Shared.Identity;
using FSH.Module.Accounting.Data;
using FSH.Module.Accounting.Domain;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.Invoices.CreateInvoice;

public record CreateInvoiceCommand(
    string InvoiceNumber,
    DateTime InvoiceDate,
    string InvoiceType,
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

public class CreateInvoiceHandler(AccountingDbContext context, ICurrentUser currentUser) 
    : ICommandHandler<CreateInvoiceCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateInvoiceCommand command, CancellationToken ct)
    {
        var entity = Invoice.Create(
            command.InvoiceNumber,
            command.InvoiceDate,
            command.InvoiceType,
            command.DueDate,
            command.BillToName,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System",
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
        
        context.Invoices.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
