using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.DebtSettlements.CreateDebtSettlement;

public sealed record CreateDebtSettlementCommand(string Name) : ICommand<Guid>;
