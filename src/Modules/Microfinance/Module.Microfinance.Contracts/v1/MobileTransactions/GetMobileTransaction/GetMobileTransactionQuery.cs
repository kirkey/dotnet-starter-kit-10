using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.MobileTransactions.GetMobileTransaction;

public sealed record GetMobileTransactionQuery(Guid Id) : IQuery<MobileTransactionDto>;
