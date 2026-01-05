using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.ShareTransactions.CreateShareTransaction;

public sealed record CreateShareTransactionCommand(string Name) : ICommand<Guid>;
