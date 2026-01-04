namespace FSH.Module.Microfinance.Contracts.v1.MobileWallets;

public record GetMobileWalletQuery(Guid Id);
public record GetMobileWalletsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive);
public record MobileWalletsPagedResponse(List<MobileWalletSummaryDto> Items, int TotalCount, int Page, int PageSize);
