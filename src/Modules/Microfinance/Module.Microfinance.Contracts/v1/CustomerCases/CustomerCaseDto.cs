namespace FSH.Module.Microfinance.Contracts.v1.CustomerCases;

public record CustomerCaseDto(
    Guid Id,
    string Name,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record CustomerCaseSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
