using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.DebtSettlements.DeleteDebtSettlement;

public sealed record DeleteDebtSettlementCommand(Guid Id) : ICommand;
