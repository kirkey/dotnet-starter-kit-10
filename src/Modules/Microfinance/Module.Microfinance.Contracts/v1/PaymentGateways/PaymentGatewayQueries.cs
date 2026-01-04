namespace FSH.Module.Microfinance.Contracts.v1.PaymentGateways;

public record GetPaymentGatewayQuery(Guid Id);
public record GetPaymentGatewaysQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive);
public record PaymentGatewaysPagedResponse(List<PaymentGatewaySummaryDto> Items, int TotalCount, int Page, int PageSize);
