using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.FeeWaivers.DeleteFeeWaiver;

namespace FSH.Module.Microfinance.Features.v1.FeeWaivers.DeleteFeeWaiver;

public class DeleteFeeWaiverHandler(MicrofinanceDbContext context) : ICommandHandler<DeleteFeeWaiverCommand>
{
    public async ValueTask<Unit> Handle(DeleteFeeWaiverCommand command, CancellationToken ct)
    {
        var entity = await context.FeeWaivers.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("FeeWaiver not found");
        
        context.FeeWaivers.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
