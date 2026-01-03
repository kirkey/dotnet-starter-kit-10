namespace FSH.Modules.Microfinance.Contracts.v1.CollectionCases;

public record CollectionCaseDto(
    Guid Id,
    string Name,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record CollectionCaseSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
