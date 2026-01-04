using FSH.Framework.Core.Identity;
using FSH.Modules.Accounting.Data;
using FSH.Modules.Accounting.Domain;
using Mediator;

namespace FSH.Modules.Accounting.Features.v1.Meters.CreateMeter;

public record CreateMeterCommand(string Name, string? Description) : ICommand<Guid>;

public class CreateMeterHandler(AccountingDbContext context, ICurrentUser currentUser) 
    : ICommandHandler<CreateMeterCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateMeterCommand command, CancellationToken ct)
    {
        var entity = Meter.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System",
            command.Description);
        
        context.Meters.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
