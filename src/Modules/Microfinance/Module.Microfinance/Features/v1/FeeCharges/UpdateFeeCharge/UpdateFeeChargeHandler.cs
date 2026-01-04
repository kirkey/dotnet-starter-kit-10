using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

namespace FSH.Module.Microfinance.Features.v1.FeeCharges.UpdateFeeCharge;

public record UpdateFeeChargeCommand(Guid Id, string Name) : ICommand<Guid>;

public class UpdateFeeChargeHandler(MicrofinanceDbContext context) : ICommandHandler<UpdateFeeChargeCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateFeeChargeCommand command, CancellationToken ct)
    {
        var entity = await context.FeeCharges.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("FeeCharge not found");
        
        entity.Update(command.Name);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
