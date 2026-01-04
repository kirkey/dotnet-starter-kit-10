using FSH.Framework.Core.Identity;
using FSH.Modules.Accounting.Data;
using FSH.Modules.Accounting.Domain;
using Mediator;

namespace FSH.Modules.Accounting.Features.v1.InvoiceLineItems.CreateInvoiceLineItem;

public record CreateInvoiceLineItemCommand(
    Guid InvoiceId,
    int LineNumber,
    string ItemDescription,
    Guid AccountId,
    string AccountCode,
    decimal Quantity,
    decimal UnitPrice,
    string? ItemCode = null,
    string? UnitOfMeasure = null,
    decimal DiscountPercent = 0,
    string? TaxCode = null,
    decimal TaxRate = 0,
    string? Notes = null) : ICommand<Guid>;

public class CreateInvoiceLineItemHandler(AccountingDbContext context, ICurrentUser currentUser) 
    : ICommandHandler<CreateInvoiceLineItemCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateInvoiceLineItemCommand command, CancellationToken ct)
    {
        var entity = InvoiceLineItem.Create(
            command.InvoiceId,
            command.LineNumber,
            command.ItemDescription,
            command.AccountId,
            command.AccountCode,
            command.Quantity,
            command.UnitPrice,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System",
            command.ItemCode,
            command.UnitOfMeasure,
            command.DiscountPercent,
            command.TaxCode,
            command.TaxRate,
            command.Notes);
        
        context.InvoiceLineItems.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
