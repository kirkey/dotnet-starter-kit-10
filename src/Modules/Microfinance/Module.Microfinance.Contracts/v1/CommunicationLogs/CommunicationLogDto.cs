namespace FSH.Module.Microfinance.Contracts.v1.CommunicationLogs;

public record CommunicationLogDto(
    Guid Id,
    string Name,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record CommunicationLogSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
