namespace FSH.Modules.Microfinance.Contracts.v1.TellerSessions;

public record GetTellerSessionQuery(Guid Id);
public record GetTellerSessionsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive);
public record TellerSessionsPagedResponse(List<TellerSessionSummaryDto> Items, int TotalCount, int Page, int PageSize);
