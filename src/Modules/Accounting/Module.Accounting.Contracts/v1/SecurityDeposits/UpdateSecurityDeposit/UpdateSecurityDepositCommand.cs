using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.SecurityDeposits.UpdateSecurityDeposit;

public sealed record UpdateSecurityDepositCommand(Guid Id, string Name, string? Description) : ICommand<Guid>;
