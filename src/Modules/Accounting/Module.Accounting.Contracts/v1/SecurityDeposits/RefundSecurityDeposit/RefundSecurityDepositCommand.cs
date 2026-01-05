using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.SecurityDeposits.RefundSecurityDeposit;

public sealed record RefundSecurityDepositCommand(Guid Id) : ICommand;
