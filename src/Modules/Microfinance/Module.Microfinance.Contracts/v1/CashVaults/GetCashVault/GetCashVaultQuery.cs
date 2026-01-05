using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.CashVaults.GetCashVault;

public sealed record GetCashVaultQuery(Guid Id) : IQuery<CashVaultDto>;
