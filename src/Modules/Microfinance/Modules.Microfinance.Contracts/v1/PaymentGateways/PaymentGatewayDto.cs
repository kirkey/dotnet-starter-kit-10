namespace FSH.Modules.Microfinance.Contracts.v1.PaymentGateways;

public record PaymentGatewayDto(
    Guid Id,
    string Name,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record PaymentGatewaySummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
