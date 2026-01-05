using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.Banks.GetListBank;

public sealed record GetBanksQuery(
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    bool? IsActive = null,
    string? CurrencyCode = null,
    bool? IsDefault = null) : IQuery<BanksPagedResponse>;

public sealed record BanksPagedResponse(
    List<BankSummaryDto> Items,
    int TotalCount,
    int Page,
    int PageSize);
