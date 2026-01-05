using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.MobileTransactions.GetMobileTransactions;

public sealed record GetMobileTransactionsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<MobileTransactionsPagedResponse>;
