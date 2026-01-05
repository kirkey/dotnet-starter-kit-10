using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.CommunicationLogs.GetCommunicationLogs;

public sealed record GetCommunicationLogsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<CommunicationLogsPagedResponse>;
