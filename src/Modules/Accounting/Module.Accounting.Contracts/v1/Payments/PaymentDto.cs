namespace FSH.Module.Accounting.Contracts.v1.Payments;

public record PaymentDto(
    Guid Id,
    string Name,
    string? Description,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record PaymentSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
