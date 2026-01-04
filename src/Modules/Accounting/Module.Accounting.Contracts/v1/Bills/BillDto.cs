namespace FSH.Module.Accounting.Contracts.v1.Bills;

public record BillDto(
    Guid Id,
    string Name,
    string? Description,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record BillSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
