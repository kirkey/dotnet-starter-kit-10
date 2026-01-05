namespace FSH.Module.Accounting.Contracts.v1.PaymentAllocations;

public record PaymentAllocationDto(
    Guid Id,
    string Name,
    string? Description,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record PaymentAllocationSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);