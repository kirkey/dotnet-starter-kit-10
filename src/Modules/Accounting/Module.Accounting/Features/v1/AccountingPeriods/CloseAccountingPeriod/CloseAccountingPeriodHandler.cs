// TODO: Implement Close operation for AccountingPeriod
using FSH.Module.Accounting.Data;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.AccountingPeriods.CloseAccountingPeriod;

public record CloseAccountingPeriodCommand(Guid Id) : ICommand;

public class CloseAccountingPeriodHandler(AccountingDbContext context) 
    : ICommandHandler<CloseAccountingPeriodCommand>
{
    public async ValueTask<Unit> Handle(CloseAccountingPeriodCommand command, CancellationToken ct)
    {
        // TODO: Implement Close logic
        throw new NotImplementedException("Close operation for AccountingPeriod needs to be implemented");
    }
}
