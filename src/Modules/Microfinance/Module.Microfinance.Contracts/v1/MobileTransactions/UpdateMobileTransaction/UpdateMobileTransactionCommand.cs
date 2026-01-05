using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.MobileTransactions.UpdateMobileTransaction;

public sealed record UpdateMobileTransactionCommand(Guid Id, string Name) : ICommand<Guid>;
