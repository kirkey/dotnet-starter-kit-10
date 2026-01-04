namespace FSH.Module.Microfinance.Contracts.v1.FeeDefinitions;

public record FeeDefinitionDto(
    Guid Id,
    string Name,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record FeeDefinitionSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
