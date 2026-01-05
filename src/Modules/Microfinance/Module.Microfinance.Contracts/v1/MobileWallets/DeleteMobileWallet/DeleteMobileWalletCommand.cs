using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.MobileWallets.DeleteMobileWallet;

public sealed record DeleteMobileWalletCommand(Guid Id) : ICommand;
