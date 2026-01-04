namespace FSH.Module.Microfinance.Contracts.v1.LegalActions;

public record LegalActionDto(
    Guid Id,
    string Name,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record LegalActionSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
