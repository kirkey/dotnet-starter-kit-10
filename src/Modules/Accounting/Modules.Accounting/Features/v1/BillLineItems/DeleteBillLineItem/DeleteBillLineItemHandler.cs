using FSH.Framework.Core.Exceptions;
using FSH.Modules.Accounting.Data;
using Mediator;

namespace FSH.Modules.Accounting.Features.v1.BillLineItems.DeleteBillLineItem;

public record DeleteBillLineItemCommand(Guid Id) : ICommand;

public class DeleteBillLineItemHandler(AccountingDbContext context) : ICommandHandler<DeleteBillLineItemCommand>
{
    public async ValueTask<Unit> Handle(DeleteBillLineItemCommand command, CancellationToken ct)
    {
        var entity = await context.BillLineItems.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("BillLineItem not found");
        
        context.BillLineItems.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
