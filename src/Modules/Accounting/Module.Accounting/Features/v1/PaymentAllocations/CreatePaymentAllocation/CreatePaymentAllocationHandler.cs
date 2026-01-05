using FSH.Framework.Shared.Identity;
using FSH.Module.Accounting.Data;
using FSH.Module.Accounting.Domain;
using Mediator;
using FSH.Module.Accounting.Contracts.v1.PaymentAllocations.CreatePaymentAllocation;

namespace FSH.Module.Accounting.Features.v1.PaymentAllocations.CreatePaymentAllocation;

public class CreatePaymentAllocationHandler(AccountingDbContext context, ICurrentUser currentUser) 
    : ICommandHandler<CreatePaymentAllocationCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreatePaymentAllocationCommand command, CancellationToken ct)
    {
        var entity = PaymentAllocation.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System",
            command.Description);
        
        context.PaymentAllocations.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
