using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.FeeCharges.DeleteFeeCharge;

namespace FSH.Module.Microfinance.Features.v1.FeeCharges.DeleteFeeCharge;

public class DeleteFeeChargeHandler(MicrofinanceDbContext context) : ICommandHandler<DeleteFeeChargeCommand>
{
    public async ValueTask<Unit> Handle(DeleteFeeChargeCommand command, CancellationToken ct)
    {
        var entity = await context.FeeCharges.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("FeeCharge not found");
        
        context.FeeCharges.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
