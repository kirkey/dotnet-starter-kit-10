using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Data;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.Bills.ApproveBill;

public record ApproveBillCommand(Guid Id) : ICommand;

public class ApproveBillHandler(AccountingDbContext context) 
    : ICommandHandler<ApproveBillCommand>
{
    public async ValueTask<Unit> Handle(ApproveBillCommand command, CancellationToken ct)
    {
        var entity = await context.Bills.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("Bill not found");

        // TODO: Implement domain approval logic (approve, record approver, etc.)

        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
