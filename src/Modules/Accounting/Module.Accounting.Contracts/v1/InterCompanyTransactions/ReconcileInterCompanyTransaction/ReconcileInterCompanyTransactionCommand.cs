using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.InterCompanyTransactions.ReconcileInterCompanyTransaction;

public sealed record ReconcileInterCompanyTransactionCommand(Guid Id) : ICommand;