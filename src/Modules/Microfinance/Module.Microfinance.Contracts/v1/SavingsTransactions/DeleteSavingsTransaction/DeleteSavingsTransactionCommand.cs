using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.SavingsTransactions.DeleteSavingsTransaction;

public sealed record DeleteSavingsTransactionCommand(Guid Id) : ICommand;
