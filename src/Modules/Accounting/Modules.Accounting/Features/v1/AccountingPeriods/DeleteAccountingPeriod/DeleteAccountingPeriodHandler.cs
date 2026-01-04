using FSH.Framework.Core.Exceptions;
using FSH.Modules.Accounting.Data;
using Mediator;

namespace FSH.Modules.Accounting.Features.v1.AccountingPeriods.DeleteAccountingPeriod;

public record DeleteAccountingPeriodCommand(Guid Id) : ICommand;

public class DeleteAccountingPeriodHandler(AccountingDbContext context) : ICommandHandler<DeleteAccountingPeriodCommand>
{
    public async ValueTask<Unit> Handle(DeleteAccountingPeriodCommand command, CancellationToken ct)
    {
        var entity = await context.AccountingPeriods.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("AccountingPeriod not found");
        
        context.AccountingPeriods.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
