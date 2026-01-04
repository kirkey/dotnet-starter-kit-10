using FSH.Framework.Shared.Identity;
using FSH.Module.Accounting.Contracts.v1.Bills.CreateBill;
using FSH.Module.Accounting.Data;
using FSH.Module.Accounting.Domain;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.Bills.CreateBill;

public class CreateBillHandler(AccountingDbContext context, ICurrentUser currentUser) 
    : ICommandHandler<CreateBillCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateBillCommand command, CancellationToken ct)
    {
        var entity = Bill.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System",
            command.Description);
        
        context.Bills.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
