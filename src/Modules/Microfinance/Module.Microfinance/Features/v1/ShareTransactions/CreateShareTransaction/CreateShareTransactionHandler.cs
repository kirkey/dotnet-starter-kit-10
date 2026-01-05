using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;
using FSH.Module.Microfinance.Domain;

using FSH.Module.Microfinance.Contracts.v1.ShareTransactions.CreateShareTransaction;

namespace FSH.Module.Microfinance.Features.v1.ShareTransactions.CreateShareTransaction;

public class CreateShareTransactionHandler(ICurrentUser currentUser,
    MicrofinanceDbContext context) : ICommandHandler<CreateShareTransactionCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateShareTransactionCommand command, CancellationToken ct)
    {
        var entity = ShareTransaction.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System");
        
        context.ShareTransactions.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
