using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.TellerSessions.GetTellerSessions;

public sealed record GetTellerSessionsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<TellerSessionsPagedResponse>;

public sealed record TellerSessionsPagedResponse(List<TellerSessionSummaryDto> Items, int TotalCount, int Page, int PageSize);
