using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.InterCompanyTransactions.DeleteInterCompanyTransaction;

public sealed record DeleteInterCompanyTransactionCommand(Guid Id) : ICommand;