using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Data;
using Mediator;
using FSH.Module.Accounting.Contracts.v1.InvoiceLineItems.UpdateInvoiceLineItem;

namespace FSH.Module.Accounting.Features.v1.InvoiceLineItems.UpdateInvoiceLineItem;

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
