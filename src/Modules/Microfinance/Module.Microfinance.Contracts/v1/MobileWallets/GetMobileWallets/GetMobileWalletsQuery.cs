using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.MobileWallets.GetMobileWallets;

public sealed record GetMobileWalletsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<MobileWalletsPagedResponse>;
