using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.InterCompanyTransactions.GetInterCompanyTransaction;

public sealed record GetInterCompanyTransactionQuery(Guid Id) : IQuery<InterCompanyTransactionDto>;