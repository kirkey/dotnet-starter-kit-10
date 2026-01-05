using FSH.Framework.Shared.Identity;
using FSH.Module.Accounting.Data;
using FSH.Module.Accounting.Domain;
using DeferredRevenueEntity = FSH.Module.Accounting.Domain.DeferredRevenue;
using Mediator;

using FSH.Module.Accounting.Contracts.v1.DeferredRevenue.CreateDeferredRevenue;namespace FSH.Module.Accounting.Features.v1.DeferredRevenue.CreateDeferredRevenue;

public class CreateDeferredRevenueHandler(AccountingDbContext context, ICurrentUser currentUser) 
    : ICommandHandler<CreateDeferredRevenueCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateDeferredRevenueCommand command, CancellationToken ct)
    {
        var entity = DeferredRevenueEntity.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System",
            command.Description);
        
        context.DeferredRevenue.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
