using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.SavingsTransactions.CreateSavingsTransaction;

public sealed record CreateSavingsTransactionCommand(string Name) : ICommand<Guid>;
