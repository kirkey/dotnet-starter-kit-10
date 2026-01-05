using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.InvestmentAccounts.GetInvestmentAccount;

public sealed record GetInvestmentAccountQuery(Guid Id) : IQuery<InvestmentAccountDto>;
