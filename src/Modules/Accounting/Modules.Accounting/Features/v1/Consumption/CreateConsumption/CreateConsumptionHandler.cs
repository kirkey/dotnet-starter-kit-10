using FSH.Framework.Core.Identity;
using FSH.Modules.Accounting.Data;
using FSH.Modules.Accounting.Domain;
using Mediator;

namespace FSH.Modules.Accounting.Features.v1.Consumption.CreateConsumption;

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
