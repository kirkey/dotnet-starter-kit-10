namespace FSH.Module.Accounting.Contracts.v1.Meters;

public record MeterDto(
    Guid Id,
    string Name,
    string? Description,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record MeterSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
