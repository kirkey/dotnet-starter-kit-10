using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.SecurityDeposits.CreateSecurityDeposit;

public sealed record CreateSecurityDepositCommand(string Name, string? Description) : ICommand<Guid>;
