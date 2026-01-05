using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.CashVaults.DeleteCashVault;

namespace FSH.Module.Microfinance.Features.v1.CashVaults.DeleteCashVault;

public class DeleteCashVaultHandler(MicrofinanceDbContext context) : ICommandHandler<DeleteCashVaultCommand>
{
    public async ValueTask<Unit> Handle(DeleteCashVaultCommand command, CancellationToken ct)
    {
        var entity = await context.CashVaults.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("CashVault not found");
        
        context.CashVaults.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
