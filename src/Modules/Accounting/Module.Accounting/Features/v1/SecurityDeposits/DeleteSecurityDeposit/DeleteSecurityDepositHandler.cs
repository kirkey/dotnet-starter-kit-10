using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Data;
using Mediator;

using FSH.Module.Accounting.Contracts.v1.SecurityDeposits.DeleteSecurityDeposit;

namespace FSH.Module.Accounting.Features.v1.SecurityDeposits.DeleteSecurityDeposit;

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
