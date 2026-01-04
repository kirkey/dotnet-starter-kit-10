using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

namespace FSH.Module.Microfinance.Features.v1.FixedDeposits.UpdateFixedDeposit;

public record UpdateFixedDepositCommand(Guid Id, string Name) : ICommand<Guid>;

public class UpdateFixedDepositHandler(MicrofinanceDbContext context) : ICommandHandler<UpdateFixedDepositCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateFixedDepositCommand command, CancellationToken ct)
    {
        var entity = await context.FixedDeposits.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("FixedDeposit not found");
        
        entity.Update(command.Name);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
