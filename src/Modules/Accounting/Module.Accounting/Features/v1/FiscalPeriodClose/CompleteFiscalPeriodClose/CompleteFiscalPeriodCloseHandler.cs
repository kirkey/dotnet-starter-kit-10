using FSH.Framework.Core.Exceptions;
using FSH.Framework.Shared.Identity;
using FSH.Module.Accounting.Data;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.FiscalPeriodClose.CompleteFiscalPeriodClose;

public record CompleteFiscalPeriodCloseCommand(Guid Id, Guid ClosingJournalEntryId) : ICommand;

public class CompleteFiscalPeriodCloseHandler(AccountingDbContext context, ICurrentUser currentUser) 
    : ICommandHandler<CompleteFiscalPeriodCloseCommand>
{
    public async ValueTask<Unit> Handle(CompleteFiscalPeriodCloseCommand command, CancellationToken ct)
    {
        var entity = await context.FiscalPeriodClose.FindAsync(command.Id, ct) ?? throw new NotFoundException("FiscalPeriodClose not found");
        entity.CompleteClose(currentUser.GetUserId(), command.ClosingJournalEntryId);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
