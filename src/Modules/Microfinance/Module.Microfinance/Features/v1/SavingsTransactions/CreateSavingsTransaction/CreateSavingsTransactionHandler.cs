using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;
using FSH.Module.Microfinance.Domain;

namespace FSH.Module.Microfinance.Features.v1.SavingsTransactions.CreateSavingsTransaction;

public record CreateSavingsTransactionCommand(string Name) : ICommand<Guid>;

public class CreateSavingsTransactionHandler(ICurrentUser currentUser,
    MicrofinanceDbContext context) : ICommandHandler<CreateSavingsTransactionCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateSavingsTransactionCommand command, CancellationToken ct)
    {
        var entity = SavingsTransaction.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System");
        
        context.SavingsTransactions.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
