using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.MobileTransactions.CreateMobileTransaction;

public sealed record CreateMobileTransactionCommand(string Name) : ICommand<Guid>;
