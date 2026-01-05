namespace FSH.Module.Accounting.Contracts.v1.RateSchedules;

public record RateScheduleDto(
    Guid Id,
    string Name,
    string? Description,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record RateScheduleSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);