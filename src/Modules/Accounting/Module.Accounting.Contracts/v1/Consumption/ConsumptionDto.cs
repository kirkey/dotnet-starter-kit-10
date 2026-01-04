namespace FSH.Module.Accounting.Contracts.v1.Consumption;

public record ConsumptionDto(
    Guid Id,
    string Name,
    string? Description,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record ConsumptionSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
