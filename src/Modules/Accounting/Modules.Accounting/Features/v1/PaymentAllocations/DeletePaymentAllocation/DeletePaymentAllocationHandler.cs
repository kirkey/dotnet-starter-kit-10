using FSH.Framework.Core.Exceptions;
using FSH.Modules.Accounting.Data;
using Mediator;

namespace FSH.Modules.Accounting.Features.v1.PaymentAllocations.DeletePaymentAllocation;

public record DeletePaymentAllocationCommand(Guid Id) : ICommand;

public class DeletePaymentAllocationHandler(AccountingDbContext context) : ICommandHandler<DeletePaymentAllocationCommand>
{
    public async ValueTask<Unit> Handle(DeletePaymentAllocationCommand command, CancellationToken ct)
    {
        var entity = await context.PaymentAllocations.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("PaymentAllocation not found");
        
        context.PaymentAllocations.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
