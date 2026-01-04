using FSH.Framework.Core.Exceptions;
using FSH.Modules.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Accounting.Features.v1.Budgets.ApproveBudget;

public record ApproveBudgetCommand(Guid Id) : ICommand;

public class ApproveBudgetHandler(AccountingDbContext context) 
    : ICommandHandler<ApproveBudgetCommand>
{
    public async ValueTask<Unit> Handle(ApproveBudgetCommand command, CancellationToken ct)
    {
        var entity = await context.Budgets.FirstOrDefaultAsync(x => x.Id == command.Id, ct)
            ?? throw new NotFoundException("Budget not found");

        // TODO: add domain-level approve behavior (set status, approver info)

        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
