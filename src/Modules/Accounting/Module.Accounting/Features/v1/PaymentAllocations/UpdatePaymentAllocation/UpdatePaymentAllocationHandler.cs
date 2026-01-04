using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Data;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.PaymentAllocations.UpdatePaymentAllocation;

public record UpdatePaymentAllocationCommand(Guid Id, string Name, string? Description) : ICommand<Guid>;

public class UpdatePaymentAllocationHandler(AccountingDbContext context) : ICommandHandler<UpdatePaymentAllocationCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdatePaymentAllocationCommand command, CancellationToken ct)
    {
        var entity = await context.PaymentAllocations.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("PaymentAllocation not found");
        
        entity.Update(command.Name, command.Description);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
