// TODO: Implement Approve operation for AccountReconciliation
using FSH.Module.Accounting.Data;
using Mediator;

using FSH.Module.Accounting.Contracts.v1.AccountReconciliations.ApproveAccountReconciliation;

namespace FSH.Module.Accounting.Features.v1.AccountReconciliations.ApproveAccountReconciliation;

public class ApproveAccountReconciliationHandler(AccountingDbContext context) 
    : ICommandHandler<ApproveAccountReconciliationCommand>
{
    public async ValueTask<Unit> Handle(ApproveAccountReconciliationCommand command, CancellationToken ct)
    {
        // TODO: Implement Approve logic
        throw new NotImplementedException("Approve operation for AccountReconciliation needs to be implemented");
    }
}
