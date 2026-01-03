namespace FSH.Modules.Microfinance.Contracts.v1.RiskCategorys;

public record RiskCategoryDto(
    Guid Id,
    string Name,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record RiskCategorySummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
