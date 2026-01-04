using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

namespace FSH.Module.Microfinance.Features.v1.SavingsAccounts.UpdateSavingsAccount;

public record UpdateSavingsAccountCommand(Guid Id, string Name) : ICommand<Guid>;

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
