using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.DebtSettlements.UpdateDebtSettlement;

public sealed record UpdateDebtSettlementCommand(Guid Id, string Name) : ICommand<Guid>;
