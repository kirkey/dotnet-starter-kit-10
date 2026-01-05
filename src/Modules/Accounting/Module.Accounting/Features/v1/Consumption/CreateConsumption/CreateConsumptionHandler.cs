using FSH.Framework.Shared.Identity;
using FSH.Module.Accounting.Data;
using FSH.Module.Accounting.Domain;
using Mediator;
using FSH.Module.Accounting.Contracts.v1.Consumption.CreateConsumption;

namespace FSH.Module.Accounting.Features.v1.Consumption.CreateConsumption;

public class CreateConsumptionHandler(AccountingDbContext context, ICurrentUser currentUser) 
    : ICommandHandler<CreateConsumptionCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateConsumptionCommand command, CancellationToken ct)
    {
        var entity = FSH.Module.Accounting.Domain.Consumption.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System",
            command.Description);
        
        context.Consumption.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
