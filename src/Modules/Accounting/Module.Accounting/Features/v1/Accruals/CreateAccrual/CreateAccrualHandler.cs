using FSH.Framework.Shared.Identity;
using FSH.Module.Accounting.Data;
using FSH.Module.Accounting.Domain;
using Mediator;
using FSH.Module.Accounting.Contracts.v1.Accruals.CreateAccrual;

namespace FSH.Module.Accounting.Features.v1.Accruals.CreateAccrual;

public class CreateAccrualHandler(AccountingDbContext context, ICurrentUser currentUser) 
    : ICommandHandler<CreateAccrualCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateAccrualCommand command, CancellationToken ct)
    {
        var entity = Accrual.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System",
            command.Description);
        
        context.Accruals.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
