using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.MobileWallets.UpdateMobileWallet;

public sealed record UpdateMobileWalletCommand(Guid Id, string Name) : ICommand<Guid>;
