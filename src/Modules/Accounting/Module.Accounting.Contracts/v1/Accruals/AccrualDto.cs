namespace FSH.Module.Accounting.Contracts.v1.Accruals;

public record AccrualDto(
    Guid Id,
    string Name,
    string? Description,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record AccrualSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
