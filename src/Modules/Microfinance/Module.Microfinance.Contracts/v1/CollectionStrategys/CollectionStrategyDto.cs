namespace FSH.Module.Microfinance.Contracts.v1.CollectionStrategys;

public record CollectionStrategyDto(
    Guid Id,
    string Name,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record CollectionStrategySummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
