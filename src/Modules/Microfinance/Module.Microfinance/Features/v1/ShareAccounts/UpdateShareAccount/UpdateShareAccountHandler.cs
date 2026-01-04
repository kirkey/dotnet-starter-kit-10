using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

namespace FSH.Module.Microfinance.Features.v1.ShareAccounts.UpdateShareAccount;

public record UpdateShareAccountCommand(Guid Id, string Name) : ICommand<Guid>;

public class UpdateShareAccountHandler(MicrofinanceDbContext context) : ICommandHandler<UpdateShareAccountCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateShareAccountCommand command, CancellationToken ct)
    {
        var entity = await context.ShareAccounts.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("ShareAccount not found");
        
        entity.Update(command.Name);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
