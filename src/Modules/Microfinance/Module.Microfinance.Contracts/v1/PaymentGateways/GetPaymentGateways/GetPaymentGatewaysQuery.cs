using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.PaymentGateways.GetPaymentGateways;

public sealed record GetPaymentGatewaysQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<PaymentGatewaysPagedResponse>;

public sealed record PaymentGatewaysPagedResponse(List<PaymentGatewaySummaryDto> Items, int TotalCount, int Page, int PageSize);
