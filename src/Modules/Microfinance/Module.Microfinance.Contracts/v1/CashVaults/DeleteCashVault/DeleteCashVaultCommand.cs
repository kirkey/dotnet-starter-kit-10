using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.CashVaults.DeleteCashVault;

public sealed record DeleteCashVaultCommand(Guid Id) : ICommand;
