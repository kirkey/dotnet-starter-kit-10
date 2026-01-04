namespace FSH.Module.Microfinance.Contracts.v1.CashVaults;

public record GetCashVaultQuery(Guid Id);
public record GetCashVaultsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive);
public record CashVaultsPagedResponse(List<CashVaultSummaryDto> Items, int TotalCount, int Page, int PageSize);
