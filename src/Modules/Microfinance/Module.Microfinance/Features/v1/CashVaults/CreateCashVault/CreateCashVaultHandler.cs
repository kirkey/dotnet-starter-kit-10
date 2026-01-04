using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;
using FSH.Module.Microfinance.Domain;

namespace FSH.Module.Microfinance.Features.v1.CashVaults.CreateCashVault;

public record CreateCashVaultCommand(string Name) : ICommand<Guid>;

public class CreateCashVaultHandler(ICurrentUser currentUser,
    MicrofinanceDbContext context) : ICommandHandler<CreateCashVaultCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateCashVaultCommand command, CancellationToken ct)
    {
        var entity = CashVault.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System");
        
        context.CashVaults.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
