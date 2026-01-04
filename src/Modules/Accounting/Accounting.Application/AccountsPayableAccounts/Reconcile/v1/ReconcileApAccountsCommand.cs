namespace Accounting.Application.AccountsPayableAccounts.Reconcile.v1;

public sealed record ReconcileApAccountsCommand(DefaultIdType Id, decimal SubsidiaryLedgerBalance) : IRequest<DefaultIdType>;

