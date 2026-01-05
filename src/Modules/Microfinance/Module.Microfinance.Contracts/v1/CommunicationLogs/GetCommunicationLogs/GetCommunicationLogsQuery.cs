using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.CommunicationLogs.GetCommunicationLogs;

public sealed record GetCommunicationLogsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<CommunicationLogsPagedResponse>;

public sealed record CommunicationLogsPagedResponse(List<CommunicationLogSummaryDto> Items, int TotalCount, int Page, int PageSize);
