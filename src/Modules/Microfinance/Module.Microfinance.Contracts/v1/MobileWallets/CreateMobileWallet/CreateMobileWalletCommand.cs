using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.MobileWallets.CreateMobileWallet;

public sealed record CreateMobileWalletCommand(string Name) : ICommand<Guid>;
