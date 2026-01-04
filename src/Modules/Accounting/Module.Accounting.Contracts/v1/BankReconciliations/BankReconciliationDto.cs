namespace FSH.Module.Accounting.Contracts.v1.BankReconciliations;

public record BankReconciliationDto(
    Guid Id,
    string ReconciliationNumber,
    Guid BankAccountId,
    DateTime StatementDate,
    decimal StatementBalance,
    decimal BookBalance,
    decimal Difference,
    string Status,
    DateTime? ReconciledDate,
    Guid? ReconciledBy,
    decimal AdjustmentAmount,
    string? AdjustmentNotes,
    string? Description,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record BankReconciliationSummaryDto(
    Guid Id,
    string ReconciliationNumber,
    Guid BankAccountId,
    DateTime StatementDate,
    decimal StatementBalance,
    decimal Difference,
    string Status,
    bool IsActive);
