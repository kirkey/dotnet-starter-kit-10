using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Data;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.Invoices.DeleteInvoice;

public record DeleteInvoiceCommand(Guid Id) : ICommand;

public class DeleteInvoiceHandler(AccountingDbContext context) : ICommandHandler<DeleteInvoiceCommand>
{
    public async ValueTask<Unit> Handle(DeleteInvoiceCommand command, CancellationToken ct)
    {
        var entity = await context.Invoices.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("Invoice not found");
        
        context.Invoices.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
