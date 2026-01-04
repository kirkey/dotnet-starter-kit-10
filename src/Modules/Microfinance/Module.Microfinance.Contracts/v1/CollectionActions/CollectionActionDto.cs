namespace FSH.Module.Microfinance.Contracts.v1.CollectionActions;

public record CollectionActionDto(
    Guid Id,
    string Name,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record CollectionActionSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
