namespace FSH.Module.Accounting.Contracts.v1.Budgets;

public record BudgetDto(
    Guid Id,
    string Name,
    string? Description,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record BudgetSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
