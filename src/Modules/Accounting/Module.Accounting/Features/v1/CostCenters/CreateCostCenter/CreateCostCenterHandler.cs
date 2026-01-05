using FSH.Framework.Shared.Identity;
using FSH.Module.Accounting.Data;
using FSH.Module.Accounting.Domain;
using Mediator;
using FSH.Module.Accounting.Contracts.v1.CostCenters.CreateCostCenter;

namespace FSH.Module.Accounting.Features.v1.CostCenters.CreateCostCenter;

public class CreateCostCenterHandler(AccountingDbContext context, ICurrentUser currentUser) 
    : ICommandHandler<CreateCostCenterCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateCostCenterCommand command, CancellationToken ct)
    {
        var entity = CostCenter.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System",
            command.Description);
        
        context.CostCenters.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
