using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.PaymentGateways.DeletePaymentGateway;

public sealed record DeletePaymentGatewayCommand(Guid Id) : ICommand;
