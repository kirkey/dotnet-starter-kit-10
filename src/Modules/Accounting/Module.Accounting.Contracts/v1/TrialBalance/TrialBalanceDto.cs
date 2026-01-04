namespace FSH.Module.Accounting.Contracts.v1.TrialBalance;

public record TrialBalanceDto(
    Guid Id,
    string Name,
    string? Description,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record TrialBalanceSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
