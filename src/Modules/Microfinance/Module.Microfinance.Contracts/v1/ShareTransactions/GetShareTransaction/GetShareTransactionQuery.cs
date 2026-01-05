using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.ShareTransactions.GetShareTransaction;

public sealed record GetShareTransactionQuery(Guid Id) : IQuery<ShareTransactionDto>;
