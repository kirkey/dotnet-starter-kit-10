using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.FixedDeposits.DeleteFixedDeposit;

public sealed record DeleteFixedDepositCommand(Guid Id) : ICommand;
