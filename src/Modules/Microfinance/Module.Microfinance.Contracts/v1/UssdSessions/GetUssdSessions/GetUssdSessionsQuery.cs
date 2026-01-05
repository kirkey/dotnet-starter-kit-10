using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.UssdSessions.GetUssdSessions;

public sealed record GetUssdSessionsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<UssdSessionsPagedResponse>;

public sealed record UssdSessionsPagedResponse(List<UssdSessionSummaryDto> Items, int TotalCount, int Page, int PageSize);
