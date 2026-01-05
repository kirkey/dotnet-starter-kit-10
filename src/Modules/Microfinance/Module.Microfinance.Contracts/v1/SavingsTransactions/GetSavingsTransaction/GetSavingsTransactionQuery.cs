using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.SavingsTransactions.GetSavingsTransaction;

public sealed record GetSavingsTransactionQuery(Guid Id) : IQuery<SavingsTransactionDto>;
