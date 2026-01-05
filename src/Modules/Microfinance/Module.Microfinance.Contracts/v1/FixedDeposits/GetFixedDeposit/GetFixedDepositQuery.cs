using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.FixedDeposits.GetFixedDeposit;

public sealed record GetFixedDepositQuery(Guid Id) : IQuery<FixedDepositDto>;
