using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

namespace FSH.Module.Microfinance.Features.v1.MobileWallets.DeleteMobileWallet;

public record DeleteMobileWalletCommand(Guid Id) : ICommand;

public class DeleteMobileWalletHandler(MicrofinanceDbContext context) : ICommandHandler<DeleteMobileWalletCommand>
{
    public async ValueTask<Unit> Handle(DeleteMobileWalletCommand command, CancellationToken ct)
    {
        var entity = await context.MobileWallets.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("MobileWallet not found");
        
        context.MobileWallets.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
