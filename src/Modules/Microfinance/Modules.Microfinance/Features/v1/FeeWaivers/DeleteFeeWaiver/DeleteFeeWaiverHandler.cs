using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.FeeWaivers.DeleteFeeWaiver;

public record DeleteFeeWaiverCommand(Guid Id) : ICommand;

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
