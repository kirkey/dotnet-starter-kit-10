namespace FSH.Module.Microfinance.Contracts.v1.FeePayments;

public record FeePaymentDto(
    Guid Id,
    string Name,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record FeePaymentSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
