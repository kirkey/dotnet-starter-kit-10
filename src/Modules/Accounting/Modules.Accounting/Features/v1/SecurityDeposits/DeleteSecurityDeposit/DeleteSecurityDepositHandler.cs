using FSH.Framework.Core.Exceptions;
using FSH.Modules.Accounting.Data;
using Mediator;

namespace FSH.Modules.Accounting.Features.v1.SecurityDeposits.DeleteSecurityDeposit;

public record DeleteSecurityDepositCommand(Guid Id) : ICommand;

public class DeleteSecurityDepositHandler(AccountingDbContext context) : ICommandHandler<DeleteSecurityDepositCommand>
{
    public async ValueTask<Unit> Handle(DeleteSecurityDepositCommand command, CancellationToken ct)
    {
        var entity = await context.SecurityDeposits.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("SecurityDeposit not found");
        
        context.SecurityDeposits.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
