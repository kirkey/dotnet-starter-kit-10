using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.CashVaults.UpdateCashVault;

public sealed record UpdateCashVaultCommand(Guid Id, string Name) : ICommand<Guid>;
