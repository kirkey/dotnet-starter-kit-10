using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

namespace FSH.Module.Microfinance.Features.v1.ShareTransactions.UpdateShareTransaction;

public record UpdateShareTransactionCommand(Guid Id, string Name) : ICommand<Guid>;

public class UpdateShareTransactionHandler(MicrofinanceDbContext context) : ICommandHandler<UpdateShareTransactionCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateShareTransactionCommand command, CancellationToken ct)
    {
        var entity = await context.ShareTransactions.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("ShareTransaction not found");
        
        entity.Update(command.Name);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
