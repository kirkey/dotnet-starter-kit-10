using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

namespace FSH.Module.Microfinance.Features.v1.FeeWaivers.UpdateFeeWaiver;

public record UpdateFeeWaiverCommand(Guid Id, string Name) : ICommand<Guid>;

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
