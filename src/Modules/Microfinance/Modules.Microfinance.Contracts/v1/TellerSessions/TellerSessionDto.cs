namespace FSH.Modules.Microfinance.Contracts.v1.TellerSessions;

public record TellerSessionDto(
    Guid Id,
    string Name,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record TellerSessionSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
