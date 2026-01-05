using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;
using FSH.Module.Microfinance.Domain;

using FSH.Module.Microfinance.Contracts.v1.AmlAlerts.CreateAmlAlert;

namespace FSH.Module.Microfinance.Features.v1.AmlAlerts.CreateAmlAlert;

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
