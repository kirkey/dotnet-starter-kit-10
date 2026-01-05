using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.FixedDeposits.UpdateFixedDeposit;

public sealed record UpdateFixedDepositCommand(Guid Id, string Name) : ICommand<Guid>;
