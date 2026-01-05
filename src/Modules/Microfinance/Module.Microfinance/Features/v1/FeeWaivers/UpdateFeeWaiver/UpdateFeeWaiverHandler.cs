using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.FeeWaivers.UpdateFeeWaiver;

namespace FSH.Module.Microfinance.Features.v1.FeeWaivers.UpdateFeeWaiver;

public class UpdateFeeWaiverHandler(MicrofinanceDbContext context) : ICommandHandler<UpdateFeeWaiverCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateFeeWaiverCommand command, CancellationToken ct)
    {
        var entity = await context.FeeWaivers.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("FeeWaiver not found");
        
        entity.Update(command.Name);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
