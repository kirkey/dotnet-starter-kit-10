namespace FSH.Modules.Microfinance.Contracts.v1.SavingsProducts;

public record SavingsProductDto(
    Guid Id,
    string Name,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record SavingsProductSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
