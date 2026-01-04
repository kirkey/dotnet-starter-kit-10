namespace FSH.Modules.Accounting.Contracts.v1.GeneralLedger;

public record GeneralLedgerDto(
    Guid Id,
    string Name,
    string? Description,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record GeneralLedgerSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
