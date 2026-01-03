namespace FSH.Modules.Microfinance.Contracts.v1.CommunicationLogs;

public record GetCommunicationLogQuery(Guid Id);
public record GetCommunicationLogsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive);
public record CommunicationLogsPagedResponse(List<CommunicationLogSummaryDto> Items, int TotalCount, int Page, int PageSize);
