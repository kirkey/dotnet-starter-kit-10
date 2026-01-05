using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.PaymentGateways.GetPaymentGateway;

public sealed record GetPaymentGatewayQuery(Guid Id) : IQuery<PaymentGatewayDto>;
