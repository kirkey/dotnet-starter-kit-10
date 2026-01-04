using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Data;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.SecurityDeposits.UpdateSecurityDeposit;

public record UpdateSecurityDepositCommand(Guid Id, string Name, string? Description) : ICommand<Guid>;

public class UpdateSecurityDepositHandler(AccountingDbContext context) : ICommandHandler<UpdateSecurityDepositCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateSecurityDepositCommand command, CancellationToken ct)
    {
        var entity = await context.SecurityDeposits.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("SecurityDeposit not found");
        
        entity.Update(command.Name, command.Description);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
