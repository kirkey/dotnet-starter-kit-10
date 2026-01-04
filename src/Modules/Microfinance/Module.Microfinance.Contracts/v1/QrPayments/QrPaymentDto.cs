namespace FSH.Module.Microfinance.Contracts.v1.QrPayments;

public record QrPaymentDto(
    Guid Id,
    string Name,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record QrPaymentSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
