namespace FSH.Modules.Microfinance.Contracts.v1.Documents;

public record DocumentDto(
    Guid Id,
    string Name,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record DocumentSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
