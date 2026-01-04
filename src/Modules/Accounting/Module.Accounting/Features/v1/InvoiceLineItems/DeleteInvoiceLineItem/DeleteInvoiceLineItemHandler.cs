using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Data;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.InvoiceLineItems.DeleteInvoiceLineItem;

public record DeleteInvoiceLineItemCommand(Guid Id) : ICommand;

public class DeleteInvoiceLineItemHandler(AccountingDbContext context) : ICommandHandler<DeleteInvoiceLineItemCommand>
{
    public async ValueTask<Unit> Handle(DeleteInvoiceLineItemCommand command, CancellationToken ct)
    {
        var entity = await context.InvoiceLineItems.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("InvoiceLineItem not found");
        
        context.InvoiceLineItems.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
