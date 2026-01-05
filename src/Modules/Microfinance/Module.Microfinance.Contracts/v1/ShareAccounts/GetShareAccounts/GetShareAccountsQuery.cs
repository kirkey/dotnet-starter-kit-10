using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.ShareAccounts.GetShareAccounts;

public sealed record GetShareAccountsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<ShareAccountsPagedResponse>;
