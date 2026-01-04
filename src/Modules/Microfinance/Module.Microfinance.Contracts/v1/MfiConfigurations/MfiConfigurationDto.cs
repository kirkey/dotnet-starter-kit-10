namespace FSH.Module.Microfinance.Contracts.v1.MfiConfigurations;

public record MfiConfigurationDto(
    Guid Id,
    string Name,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record MfiConfigurationSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
