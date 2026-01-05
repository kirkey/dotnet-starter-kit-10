using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.SecurityDeposits.DeleteSecurityDeposit;

public sealed record DeleteSecurityDepositCommand(Guid Id) : ICommand;
