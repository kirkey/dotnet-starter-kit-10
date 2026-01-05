using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.ShareTransactions.UpdateShareTransaction;

public sealed record UpdateShareTransactionCommand(Guid Id, string Name) : ICommand<Guid>;
