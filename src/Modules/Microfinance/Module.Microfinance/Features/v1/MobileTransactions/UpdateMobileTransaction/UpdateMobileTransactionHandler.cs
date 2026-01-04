using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

namespace FSH.Module.Microfinance.Features.v1.MobileTransactions.UpdateMobileTransaction;

public record UpdateMobileTransactionCommand(Guid Id, string Name) : ICommand<Guid>;

public class UpdateMobileTransactionHandler(MicrofinanceDbContext context) : ICommandHandler<UpdateMobileTransactionCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateMobileTransactionCommand command, CancellationToken ct)
    {
        var entity = await context.MobileTransactions.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("MobileTransaction not found");
        
        entity.Update(command.Name);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
