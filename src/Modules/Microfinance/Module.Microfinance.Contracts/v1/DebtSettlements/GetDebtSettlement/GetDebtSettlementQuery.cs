using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.DebtSettlements.GetDebtSettlement;

public sealed record GetDebtSettlementQuery(Guid Id) : IQuery<DebtSettlementDto>;
