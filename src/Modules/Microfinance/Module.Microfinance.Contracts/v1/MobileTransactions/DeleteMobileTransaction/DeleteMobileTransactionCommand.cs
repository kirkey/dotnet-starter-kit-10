using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.MobileTransactions.DeleteMobileTransaction;

public sealed record DeleteMobileTransactionCommand(Guid Id) : ICommand;
