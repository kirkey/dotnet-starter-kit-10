// TODO: Implement Reconcile operation for InterCompanyTransaction
using FSH.Modules.Accounting.Data;
using Mediator;

namespace FSH.Modules.Accounting.Features.v1.InterCompanyTransactions.ReconcileInterCompanyTransaction;

public record ReconcileInterCompanyTransactionCommand(Guid Id) : ICommand;

public class ReconcileInterCompanyTransactionHandler(AccountingDbContext context) 
    : ICommandHandler<ReconcileInterCompanyTransactionCommand>
{
    public async ValueTask<Unit> Handle(ReconcileInterCompanyTransactionCommand command, CancellationToken ct)
    {
        // TODO: Implement Reconcile logic
        throw new NotImplementedException("Reconcile operation for InterCompanyTransaction needs to be implemented");
    }
}
