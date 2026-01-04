using FSH.Framework.Shared.Identity;
using FSH.Module.Accounting.Data;
using FSH.Module.Accounting.Domain;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.Consumption.CreateConsumption;

public record CreateConsumptionCommand(string Name, string? Description) : ICommand<Guid>;

public class CreateConsumptionHandler(AccountingDbContext context, ICurrentUser currentUser) 
    : ICommandHandler<CreateConsumptionCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateConsumptionCommand command, CancellationToken ct)
    {
        var entity = Consumption.Create(
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
