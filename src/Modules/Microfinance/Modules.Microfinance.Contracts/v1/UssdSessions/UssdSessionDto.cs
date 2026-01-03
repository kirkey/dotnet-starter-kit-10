namespace FSH.Modules.Microfinance.Contracts.v1.UssdSessions;

public record UssdSessionDto(
    Guid Id,
    string Name,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record UssdSessionSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
