using FSH.Framework.Core.Exceptions;
using FSH.Framework.Shared.Identity;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.BankReconciliations.ApproveBankReconciliation;

public record ApproveBankReconciliationCommand(Guid Id) : ICommand;

public class ApproveBankReconciliationHandler(AccountingDbContext context, ICurrentUser currentUser) 
    : ICommandHandler<ApproveBankReconciliationCommand>
{
    public async ValueTask<Unit> Handle(ApproveBankReconciliationCommand command, CancellationToken ct)
    {
        var entity = await context.BankReconciliations.FirstOrDefaultAsync(x => x.Id == command.Id, ct)
            ?? throw new NotFoundException("BankReconciliation not found");

        entity.Reconcile(currentUser.GetUserId());
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
