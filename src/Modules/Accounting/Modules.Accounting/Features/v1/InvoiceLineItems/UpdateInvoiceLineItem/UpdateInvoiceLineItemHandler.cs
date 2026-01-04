using FSH.Framework.Core.Exceptions;
using FSH.Modules.Accounting.Data;
using Mediator;

namespace FSH.Modules.Accounting.Features.v1.InvoiceLineItems.UpdateInvoiceLineItem;

public record UpdateInvoiceLineItemCommand(
    Guid Id,
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

public class UpdateInvoiceLineItemHandler(AccountingDbContext context) : ICommandHandler<UpdateInvoiceLineItemCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateInvoiceLineItemCommand command, CancellationToken ct)
    {
        var entity = await context.InvoiceLineItems.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("InvoiceLineItem not found");
        
        entity.Update(
            command.LineNumber,
            command.ItemDescription,
            command.AccountId,
            command.AccountCode,
            command.Quantity,
            command.UnitPrice,
            command.ItemCode,
            command.UnitOfMeasure,
            command.DiscountPercent,
            command.TaxCode,
            command.TaxRate,
            command.Notes);
        
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
