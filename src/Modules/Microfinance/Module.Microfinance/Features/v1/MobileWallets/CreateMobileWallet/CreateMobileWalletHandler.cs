using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;
using FSH.Module.Microfinance.Domain;

namespace FSH.Module.Microfinance.Features.v1.MobileWallets.CreateMobileWallet;

public record CreateMobileWalletCommand(string Name) : ICommand<Guid>;

public class CreateMobileWalletHandler(ICurrentUser currentUser,
    MicrofinanceDbContext context) : ICommandHandler<CreateMobileWalletCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateMobileWalletCommand command, CancellationToken ct)
    {
        var entity = MobileWallet.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System");
        
        context.MobileWallets.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
