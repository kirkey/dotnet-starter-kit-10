namespace FSH.Modules.Accounting.Contracts.v1.SecurityDeposits;

public record SecurityDepositDto(
    Guid Id,
    string Name,
    string? Description,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record SecurityDepositSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
