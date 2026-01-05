using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.ShareTransactions.DeleteShareTransaction;

public sealed record DeleteShareTransactionCommand(Guid Id) : ICommand;
