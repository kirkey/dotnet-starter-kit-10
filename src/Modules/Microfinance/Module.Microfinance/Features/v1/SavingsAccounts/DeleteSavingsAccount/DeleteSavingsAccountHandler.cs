using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.SavingsAccounts;namespace FSH.Module.Microfinance.Features.v1.SavingsAccounts.DeleteSavingsAccount;

public class DeleteSavingsAccountHandler(MicrofinanceDbContext context) : ICommandHandler<DeleteSavingsAccountCommand>
{
    public async ValueTask<Unit> Handle(DeleteSavingsAccountCommand command, CancellationToken ct)
    {
        var entity = await context.SavingsAccounts.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("SavingsAccount not found");
        
        context.SavingsAccounts.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
