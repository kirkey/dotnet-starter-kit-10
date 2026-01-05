using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.CashVaults.GetCashVaults;

public sealed record GetCashVaultsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<CashVaultsPagedResponse>;

public sealed record CashVaultsPagedResponse(List<CashVaultSummaryDto> Items, int TotalCount, int Page, int PageSize);
