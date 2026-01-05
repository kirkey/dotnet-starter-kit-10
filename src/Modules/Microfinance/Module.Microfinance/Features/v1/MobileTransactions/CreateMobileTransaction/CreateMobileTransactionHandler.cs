using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;
using FSH.Module.Microfinance.Domain;

using FSH.Module.Microfinance.Contracts.v1.MobileTransactions.CreateMobileTransaction;

namespace FSH.Module.Microfinance.Features.v1.MobileTransactions.CreateMobileTransaction;

public class CreateMobileTransactionHandler(ICurrentUser currentUser,
    MicrofinanceDbContext context) : ICommandHandler<CreateMobileTransactionCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateMobileTransactionCommand command, CancellationToken ct)
    {
        var entity = MobileTransaction.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System");
        
        context.MobileTransactions.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
