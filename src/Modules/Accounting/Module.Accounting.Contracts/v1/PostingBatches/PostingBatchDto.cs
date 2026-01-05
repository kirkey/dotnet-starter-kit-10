namespace FSH.Module.Accounting.Contracts.v1.PostingBatches;

public record PostingBatchDto(
    Guid Id,
    string Name,
    DateTime BatchDate,
    string? Description,
    string Status,
    decimal TotalDebits,
    decimal TotalCredits,
    int EntryCount,
    DateTimeOffset? PostedOn,
    Guid? PostedBy,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record PostingBatchSummaryDto(
    Guid Id,
    string Name,
    DateTime BatchDate,
    string Status,
    int EntryCount,
    decimal TotalDebits,
    decimal TotalCredits,
    bool IsActive);