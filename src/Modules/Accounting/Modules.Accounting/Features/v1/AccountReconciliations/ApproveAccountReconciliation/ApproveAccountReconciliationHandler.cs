// TODO: Implement Approve operation for AccountReconciliation
using FSH.Modules.Accounting.Data;
using Mediator;

namespace FSH.Modules.Accounting.Features.v1.AccountReconciliations.ApproveAccountReconciliation;

public record ApproveAccountReconciliationCommand(Guid Id) : ICommand;

public class ApproveAccountReconciliationHandler(AccountingDbContext context) 
    : ICommandHandler<ApproveAccountReconciliationCommand>
{
    public async ValueTask<Unit> Handle(ApproveAccountReconciliationCommand command, CancellationToken ct)
    {
        // TODO: Implement Approve logic
        throw new NotImplementedException("Approve operation for AccountReconciliation needs to be implemented");
    }
}
