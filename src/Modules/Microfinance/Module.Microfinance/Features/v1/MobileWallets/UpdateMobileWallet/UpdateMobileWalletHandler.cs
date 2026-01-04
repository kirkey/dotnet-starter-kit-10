using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

namespace FSH.Module.Microfinance.Features.v1.MobileWallets.UpdateMobileWallet;

public record UpdateMobileWalletCommand(Guid Id, string Name) : ICommand<Guid>;

public class UpdateMobileWalletHandler(MicrofinanceDbContext context) : ICommandHandler<UpdateMobileWalletCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateMobileWalletCommand command, CancellationToken ct)
    {
        var entity = await context.MobileWallets.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("MobileWallet not found");
        
        entity.Update(command.Name);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
