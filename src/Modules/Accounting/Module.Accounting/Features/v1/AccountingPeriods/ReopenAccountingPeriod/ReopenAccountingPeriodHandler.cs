// TODO: Implement Reopen operation for AccountingPeriod
using FSH.Module.Accounting.Data;
using Mediator;

using FSH.Module.Accounting.Contracts.v1.AccountingPeriods.ReopenAccountingPeriod;

namespace FSH.Module.Accounting.Features.v1.AccountingPeriods.ReopenAccountingPeriod;

public class ReopenAccountingPeriodHandler(AccountingDbContext context) 
    : ICommandHandler<ReopenAccountingPeriodCommand>
{
    public async ValueTask<Unit> Handle(ReopenAccountingPeriodCommand command, CancellationToken ct)
    {
        // TODO: Implement Reopen logic
        throw new NotImplementedException("Reopen operation for AccountingPeriod needs to be implemented");
    }
}
