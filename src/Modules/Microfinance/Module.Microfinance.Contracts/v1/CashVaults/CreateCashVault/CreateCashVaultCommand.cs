using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.CashVaults.CreateCashVault;

public sealed record CreateCashVaultCommand(string Name) : ICommand<Guid>;
