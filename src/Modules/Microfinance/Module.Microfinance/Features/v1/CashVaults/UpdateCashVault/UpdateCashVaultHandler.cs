using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

namespace FSH.Module.Microfinance.Features.v1.CashVaults.UpdateCashVault;

public record UpdateCashVaultCommand(Guid Id, string Name) : ICommand<Guid>;

public class UpdateCashVaultHandler(MicrofinanceDbContext context) : ICommandHandler<UpdateCashVaultCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateCashVaultCommand command, CancellationToken ct)
    {
        var entity = await context.CashVaults.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("CashVault not found");
        
        entity.Update(command.Name);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
