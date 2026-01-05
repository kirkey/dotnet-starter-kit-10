using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.FixedDeposits.CreateFixedDeposit;

public sealed record CreateFixedDepositCommand(string Name) : ICommand<Guid>;
