using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.AmlAlerts.UpdateAmlAlert;

namespace FSH.Module.Microfinance.Features.v1.AmlAlerts.UpdateAmlAlert;

public class UpdateAmlAlertHandler(MicrofinanceDbContext context) : ICommandHandler<UpdateAmlAlertCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateAmlAlertCommand command, CancellationToken ct)
    {
        var entity = await context.AmlAlerts.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("AmlAlert not found");
        
        entity.Update(command.Name);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
