namespace FSH.Module.Microfinance.Contracts.v1.UssdSessions;

public record GetUssdSessionQuery(Guid Id);
public record GetUssdSessionsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive);
public record UssdSessionsPagedResponse(List<UssdSessionSummaryDto> Items, int TotalCount, int Page, int PageSize);
