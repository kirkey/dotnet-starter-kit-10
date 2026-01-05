using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.FixedDeposits.DeleteFixedDeposit;

namespace FSH.Module.Microfinance.Features.v1.FixedDeposits.DeleteFixedDeposit;

public class DeleteFixedDepositHandler(MicrofinanceDbContext context) : ICommandHandler<DeleteFixedDepositCommand>
{
    public async ValueTask<Unit> Handle(DeleteFixedDepositCommand command, CancellationToken ct)
    {
        var entity = await context.FixedDeposits.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("FixedDeposit not found");
        
        context.FixedDeposits.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
