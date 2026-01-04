namespace FSH.Module.Accounting.Contracts.v1.AccountReconciliations;

public record AccountReconciliationDto(
    Guid Id,
    string Name,
    string? Description,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record AccountReconciliationSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
