using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.SavingsAccounts;namespace FSH.Module.Microfinance.Features.v1.SavingsAccounts.UpdateSavingsAccount;

public class UpdateSavingsAccountHandler(MicrofinanceDbContext context) : ICommandHandler<UpdateSavingsAccountCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateSavingsAccountCommand command, CancellationToken ct)
    {
        var entity = await context.SavingsAccounts.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("SavingsAccount not found");
        
        entity.Update(command.Name);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
