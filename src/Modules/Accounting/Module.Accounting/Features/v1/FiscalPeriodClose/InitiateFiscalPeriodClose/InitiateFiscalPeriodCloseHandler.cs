using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Data;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.FiscalPeriodClose.InitiateFiscalPeriodClose;

public record InitiateFiscalPeriodCloseCommand(Guid Id) : ICommand;

public class InitiateFiscalPeriodCloseHandler(AccountingDbContext context) 
    : ICommandHandler<InitiateFiscalPeriodCloseCommand>
{
    public async ValueTask<Unit> Handle(InitiateFiscalPeriodCloseCommand command, CancellationToken ct)
    {
        var entity = await context.FiscalPeriodClose.FindAsync(command.Id, ct) ?? throw new NotFoundException("FiscalPeriodClose not found");
        entity.BeginClose();
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
