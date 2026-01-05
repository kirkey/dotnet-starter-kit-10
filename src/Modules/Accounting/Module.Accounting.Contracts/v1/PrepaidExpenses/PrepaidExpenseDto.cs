namespace FSH.Module.Accounting.Contracts.v1.PrepaidExpenses;

public record PrepaidExpenseDto(
    Guid Id,
    string Name,
    string? Description,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record PrepaidExpenseSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);