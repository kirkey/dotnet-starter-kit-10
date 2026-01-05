using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.SavingsTransactions.UpdateSavingsTransaction;

public sealed record UpdateSavingsTransactionCommand(Guid Id, string Name) : ICommand<Guid>;
