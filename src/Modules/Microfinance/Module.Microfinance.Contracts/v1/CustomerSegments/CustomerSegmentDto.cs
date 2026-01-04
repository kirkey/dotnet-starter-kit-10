namespace FSH.Module.Microfinance.Contracts.v1.CustomerSegments;

public record CustomerSegmentDto(
    Guid Id,
    string Name,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record CustomerSegmentSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
