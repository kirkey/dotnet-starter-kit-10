using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.MobileWallets.GetMobileWallet;

public sealed record GetMobileWalletQuery(Guid Id) : IQuery<MobileWalletDto>;
