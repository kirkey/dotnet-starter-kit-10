namespace Accounting.Application.AccountsReceivableAccounts.Reconcile.v1;

public sealed record ReconcileArAccountsCommand(DefaultIdType Id, decimal SubsidiaryLedgerBalance) : IRequest<DefaultIdType>;

