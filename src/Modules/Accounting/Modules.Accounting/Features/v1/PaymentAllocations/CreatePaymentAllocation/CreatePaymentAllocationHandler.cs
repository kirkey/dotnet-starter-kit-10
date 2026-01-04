using FSH.Framework.Core.Identity;
using FSH.Modules.Accounting.Data;
using FSH.Modules.Accounting.Domain;
using Mediator;

namespace FSH.Modules.Accounting.Features.v1.PaymentAllocations.CreatePaymentAllocation;

public record CreatePaymentAllocationCommand(string Name, string? Description) : ICommand<Guid>;

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
