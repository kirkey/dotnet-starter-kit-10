namespace FSH.Modules.Accounting.Contracts.v1.BudgetDetails;

public record BudgetDetailDto(
    Guid Id,
    string Name,
    string? Description,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record BudgetDetailSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
