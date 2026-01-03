using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;
using FSH.Modules.Microfinance.Domain;

namespace FSH.Modules.Microfinance.Features.v1.AmlAlerts.CreateAmlAlert;

public record CreateAmlAlertCommand(string Name) : ICommand<Guid>;

public class CreateAmlAlertHandler(ICurrentUser currentUser,
    MicrofinanceDbContext context) : ICommandHandler<CreateAmlAlertCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateAmlAlertCommand command, CancellationToken ct)
    {
        var entity = AmlAlert.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System");
        
        context.AmlAlerts.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
