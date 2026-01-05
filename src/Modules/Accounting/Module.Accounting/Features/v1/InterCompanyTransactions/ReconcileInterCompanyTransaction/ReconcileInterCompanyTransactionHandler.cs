// TODO: Implement Reconcile operation for InterCompanyTransaction
using FSH.Module.Accounting.Data;
using Mediator;

using FSH.Module.Accounting.Contracts.v1.InterCompanyTransactions.ReconcileInterCompanyTransaction;namespace FSH.Module.Accounting.Features.v1.InterCompanyTransactions.ReconcileInterCompanyTransaction;

public class ReconcileInterCompanyTransactionHandler(AccountingDbContext context) 
    : ICommandHandler<ReconcileInterCompanyTransactionCommand>
{
    public async ValueTask<Unit> Handle(ReconcileInterCompanyTransactionCommand command, CancellationToken ct)
    {
        // TODO: Implement Reconcile logic
        throw new NotImplementedException("Reconcile operation for InterCompanyTransaction needs to be implemented");
    }
}
