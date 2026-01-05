using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Data;
using Mediator;
using FSH.Module.Accounting.Contracts.v1.BillLineItems.DeleteBillLineItem;

namespace FSH.Module.Accounting.Features.v1.BillLineItems.DeleteBillLineItem;

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
